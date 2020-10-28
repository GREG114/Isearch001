using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isearch.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Isearch.Controllers
{
    [Route("api/tool")]
    [ApiController]
    public class tool : ControllerBase
    {
        public ElasticClient client;

        public tool(IElastic elasticSearch)
        {            
            var client = elasticSearch.GetClient();
        }
        //[HttpGet]
        //public ActionResult<string> Get(string start,string end)
        //{
        //    var s = DateTime.Parse(start);
        //    var e = DateTime.Parse(end);
        //    var r = e - s;
        //    return r.ToString();
        //}




        //[HttpGet("{arg}")]
        //public ActionResult<string> Get(string arg)
        //{
        //    ///.net可以直接吃来自python的时间格式的字符串!!
        //    var test = DateTime.Parse(arg);

        //    return arg;
        //}
    }
}
