using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isearch.Models;
using Isearch.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Isearch.Controllers
{
    public class APIController : Controller
    {
        private Kcontext kingdee;
        public ElasticClient client;
        private List<Dictionary<string, dynamic>>  workdaylist;
        public APIController(IElastic elasticSearch,Kcontext kingdee)
        {
            client = elasticSearch.GetClient();
            this.kingdee = kingdee;
            this.workdaylist = client.Search<Dictionary<string, dynamic>>(s => s.Index("workday").Size(9999)).Documents.ToList();
        }
        
        [Authorize(Roles = "CrmDeveloper")]
        public IActionResult RPAVouchers(int fPeriod,int fYear)
        {
            //return Json(kingdee.t_Voucher.ToList());
            return Json(kingdee.t_Voucher.Where(c=>c.FPreparerID==16400&&c.FPeriod==fPeriod&&c.FYear==fYear).ToList());
        }



        // GET: /<controller>/
        [HttpGet]
        public IActionResult Test()
        {//.QueryOnQueryString("name.keyword :" + keyword))
            var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("cufull").Size(3)).Documents;

            return Json(cus);
        }
        /// <summary>
        /// 返回的是秒数,自己回去计算吧
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<double> workhours(string start, string end)
        {
            var s = DateTime.Parse(start);
            var e = DateTime.Parse(end);
            var datelist = workdaylist.Where(c => c["workday"]);
            double result = 0;
            while (s < e)
            {
                var str = s.ToString("yyyyMMdd");
                //创建临时变量用来当作改日的开始变量进行计算,s进入循环
                var cacstart = s;
                //准备四个时间节点用来计算
                var shangban = new DateTime(cacstart.Year, cacstart.Month, cacstart.Day, 9, 0, 0);
                var xiaban = new DateTime(cacstart.Year, cacstart.Month, cacstart.Day, 17, 30, 0);
                var wuxiu = new DateTime(cacstart.Year, cacstart.Month, cacstart.Day, 12, 0, 0);
                var xiawushangban = new DateTime(cacstart.Year, cacstart.Month, cacstart.Day, 13, 0, 0);
                var cacend = xiaban;
                if (cacstart.Date == e.Date)
                {
                    cacend = e;
                    if (e < shangban)
                    {
                        break;
                    }
                }
                if (datelist.Where(c => c["date"] == s.ToString("yyyyMMdd")).Count() > 0)
                {
                    
                    //上班前
                    if (cacstart < shangban)
                    {
                        if (cacend <= wuxiu) { result += (cacend - shangban).TotalSeconds; }
                        if (cacend > wuxiu && cacend<xiawushangban) { result += (wuxiu - shangban).TotalSeconds;  }
                        if (cacend >= xiawushangban && cacend <xiaban) { 
                            result += (cacend - shangban).TotalSeconds - 3600;  }
                        if (cacend >= xiaban) { 
                            result += (xiaban - shangban).TotalSeconds - 3600;  }
                    }
                    //午休前
                    if (cacstart >= shangban && cacstart<wuxiu)
                    {
                        if (cacend < wuxiu) { result += (cacend - cacstart).TotalSeconds;  }
                        if (cacend >= wuxiu&&cacend<xiawushangban) { result += (wuxiu - cacstart).TotalSeconds;  }
                        if (cacend >= xiawushangban&&cacend<=xiaban) 
                        { 
                            result += (cacend - cacstart).TotalSeconds -3600;  
                        }
                        if (cacend >xiaban) { result += (xiaban - cacstart).TotalSeconds - 3600;  }

                    }
                    //下午上班前
                    if (cacstart >= wuxiu && cacstart < xiawushangban)
                    {
                        if (cacend>=xiawushangban && cacend <= xiaban) { result += (cacend - xiawushangban).TotalSeconds;  }
                        if (cacend > xiaban) { result += (xiaban - wuxiu).TotalSeconds;  }
                    }
                    //下班前
                    if(cacstart>=xiawushangban && cacstart < xiaban)
                    {
                        if (cacend <= xiaban) { result += (cacend - cacstart).TotalSeconds;  }
                        if (cacend > xiaban) { result += (xiaban - cacstart).TotalSeconds;  }
                    }
                }
                s = shangban;
                s = s.AddDays(1);
            }
            return result;
        }
    }
}
