using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Isearch.Models;
using Isearch.services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Isearch.Controllers
{
    public partial class CRMAnalysisController : Controller
    {
        private readonly IHostingEnvironment hostingEnv;

        public CRMAnalysisController(IHostingEnvironment env)
        {
            hostingEnv = env;
        }
        [HttpPost]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {
            
            foreach (var file in files)
            {
                long size = 0;
                var filename = ContentDispositionHeaderValue
                               .Parse(file.ContentDisposition)
                               .FileName
                               .Trim('"');
                //这个hostingEnv.WebRootPath就是要存的地址可以改下
                filename = hostingEnv.WebRootPath + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            return null;
        }
        public IActionResult Index()
        {

            return View();
       


        }
        public dynamic ShowStep(string mod, string modfull, ElkMaster master)
        {
            List<string> 阶段 = new List<string>();
            string 阶段str = "";
            //定义不同模型的阶段，暂时写死后期想办法
            switch (mod)
            {

                case "opstage":
                    阶段.Add("初步接洽");
                    阶段.Add("需求确定");
                    阶段.Add("测试");
                    阶段.Add("方案/报价");
                    阶段.Add("投标");
                    阶段.Add("谈判/合同");
                    阶段.Add("赢单");
                    阶段.Add("输单");
                    阶段str = "stage_mapped";
                    break;
                case "custage":
                    阶段.Add("待确认");
                    阶段.Add("待约见");
                    阶段.Add("初访");
                    阶段.Add("合作意向强");
                    阶段.Add("培训");
                    阶段.Add("签代理协议");
                    阶段.Add("成交");
                    阶段.Add("未成交");
                    阶段str = "status_mapped";
                    break;
                default: break;
            }
            var logs = master.ElkData(0, mod);
            List<string> ids = new List<string>();
            foreach (var o in logs)
            {
                if (ids.Where(c => c == o["实体id"].ToString()).Count() == 0)
                {
                    ids.Add(o["实体id"].ToString());
                }
            }
            var obj = master.client.MultiGet(c => c.Index(modfull).GetMany<dynamic>(ids).SourceEnabled(true));
            var list = JArray.FromObject(obj.Hits);

            List<int> logcount = new List<int>();
            List<int> currentcount = new List<int>();
            foreach (var i in 阶段)
            {
                //分别获取处于该阶段下所有商机【含曾经】所处的数量
                var count = logs.Where(c => c["当前阶段"].ToString() == i).Count();
                //获取这些商机的当前阶段
                var ccount = list.Where(c => c["_source"][阶段str].ToString() == i).Count();
                logcount.Add(count - ccount);
                currentcount.Add(ccount);
            }
            阶段.Sort((left, right) => {
                return -1;
            });
            logcount.Sort((left, right) => {
                return -1;
            });
            currentcount.Sort((left, right) => {
                return -1;
            });
            var result = new { 阶段, logcount, currentcount };

            //实际需要的数据 阶段字段列表  所有计数的数组    当前实际计数的数组 都得倒序
            return result;

        }

        [HttpGet]
        public IActionResult CrmLog1()
        {
            string url = "http://ip.lxgreg.cn:19200";
            string user = "elastic";
            string pass = "duan1212";
            ElkMaster master = new ElkMaster(user, pass, url);

            return Json(ShowStep("custage","cufull",master));
        }



        //[HttpGet]
        //public IActionResult CrmLog()
        //{

        //    string url = "http://ip.lxgreg.cn:19200";
        //    string user = "elastic";
        //    string pass = "duan1212";
        //    string model = "cufull";
        //    //定义返回字段
        //    string[] fields = { "id", "实体id", "更新时间", "当前阶段" };
        //    string[] cufields = { "id", "status_mapped", "category_mapped", "name" }; ;
        //    //大概三秒
        //    ElkMaster master = new ElkMaster(user, pass, url);

        //    //  
        //    var M = master.ElkData(0, "custage", fields);

        //    ////取当前客户状态耗时9秒
        //    var N = master.ElkData(0, "cufull", cufields);
        //    string[] 阶段 = { "成交", "签代理协议", "培训", "合作意向强", "初访", "待约见", "待确认" };
        //    List<int> 阶段数量 = new List<int>();
        //    List<int> 实际阶段数量 = new List<int>();

            
        //    foreach (var i in 阶段)
        //    {
        //        //实际阶段数量
        //        int realcount = N.Where(c => c["status_mapped"].ToString() == i).Count();

        //        //曾经处于这个阶段的数量
        //        int count = M.Where(c => c["当前阶段"].ToString() == i).Count();
        //        int staycount = count - realcount;
        //        阶段数量.Add(staycount);
        //        实际阶段数量.Add(realcount);
        //    }

        //    return (Json(new { 阶段, 阶段数量 }));

        //    //var cc = N.Where(c => c["category_mapped"].ToString() == "初访").Count();

        //    ////聚合运算 非常快就可以完成
        //    //var result = M.Join(N, a => a["实体id"].ToString(), b => b["id"].ToString(), (a, b) => new
        //    //{
        //    //    日志id = a["id"],
        //    //    商机id = a["实体id"],
        //    //    客户类型 = b["category_mapped"],
        //    //    日志时间戳 = a["更新时间"],
        //    //    阶段 = a["当前阶段"],
        //    //    最新阶段 = b["status_mapped"]
        //    //});


        //    //List<int> 阶段数量 = new List<int>();
        //    //List<int> 已推进 = new List<int>();
        //    //foreach(var i in 阶段)
        //    //{
        //    //    int count = result.Where(c => c.阶段.ToString() == i).Count();
        //    //    阶段数量.Add(count);

        //    //}


        //    //return (Json(new { 阶段,  阶段数量 }));





        //}


    }
}