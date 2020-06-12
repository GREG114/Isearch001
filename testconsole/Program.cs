using Isearch.Controllers;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Console;

namespace testconsole
{
    class Program
    {
        public static ElasticClient client = new ElasticClient(
                   new ConnectionSettings(new Uri("http://ip.lxgreg.cn:19200")).BasicAuthentication("elastic", "duan1212")
                   );
        static void Main(string[] args)
        {

            string[] status = { "培训", "签代理协议", "成交" };
            string[] status1 = { "培训", "签代理协议", "成交" };
            string keyword = "上海光勇信息技术有限公司";
            
            AKCrm ak = new AKCrm("17091923541", "duan1212");

            var test = "四川虹信软件股份有限公司";
            var cu = getCustomer(test);

            WriteLine(cu["id"]);
       
        }


        static JToken getCustomer(string keyword)
        {
            var cus = client.Search<Dictionary<string,dynamic>>(s => s.Index("cufull").QueryOnQueryString("name.keyword :" + keyword)).Documents;
            return JObject.FromObject(cus.FirstOrDefault());
        }
    }
}

////var client = new ElasticClient(
////    new ConnectionSettings(new Uri("http://ip.lxgreg.cn:19200")).BasicAuthentication("elastic", "duan1212")
////    );

////var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("cufull")).Documents;
//var obj = new { name = new { china = "中文", english = "english" }, age = 16 };
//var jobj = JObject.FromObject(obj);
//jobj.Add("test",JObject.FromObject(new { test = "haha" }));
//            WriteLine(jobj);

//ReadKey();
