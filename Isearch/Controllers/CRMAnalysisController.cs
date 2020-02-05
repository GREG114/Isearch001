using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isearch.Models;
using Isearch.services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Isearch.Controllers
{
    public partial class CRMAnalysisController : Controller
    {
        public IActionResult Index()
        {

            return View();
       


        }
        [HttpGet]
        public IActionResult CrmLog()
        {

            string url = "http://ip.lxgreg.cn:19200";
            string user = "elastic";
            string pass = "duan1212";
            string model = "cufull";
            //定义返回字段
            string[] fields = { "id", "实体id", "更新时间", "当前阶段" };
            string[] cufields = { "id", "status_mapped", "category_mapped", "name" }; ;
            //大概三秒
            ElkMaster master = new ElkMaster(user, pass, url);
            var M = master.ElkData(0, "custage", fields);
            var N = master.ElkData(0, "cufull", cufields);

            var result = M.Join(N, a => a["实体id"].ToString(), b => b["id"].ToString(), (a, b) => new
            {
                日志id = a["id"],
                商机id = a["实体id"],
                客户类型 = b["category_mapped"],
                日志时间戳 = a["更新时间"],
                阶段 = a["当前阶段"],
                最新阶段 = b["status_mapped"]
            });

            //先解决有没有的问题
            string[] 阶段 = {  "成交", "签代理协议", "培训", "合作意向强", "初访", "待约见", "待确认" };

            List<int> 阶段数量 = new List<int>();
            List<int> 已推进 = new List<int>();
            foreach(var i in 阶段)
            {
                int count = result.Where(c => c.最新阶段.ToString() == i).Count();
                阶段数量.Add(count);

            }
            return (Json(new { 阶段,  阶段数量 }));





        }


    }
}