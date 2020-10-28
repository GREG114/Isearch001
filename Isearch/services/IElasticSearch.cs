using Isearch.Controllers;
using Nest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Isearch.services
{
    public interface IElastic
    {
        ElasticClient GetClient();
    }

    public class Elastic : IElastic{
        ElasticClient client = null;
        public Elastic()
        {
            client = new ElasticClient(
                new ConnectionSettings(new Uri("http://ip.lxgreg.cn:19200")).BasicAuthentication("elastic", "duan1212")
                );
        }
        public ElasticClient GetClient()
        {
            return client;
        }
    }

    public interface IAKHelper
    {
        AKCrm GetAKCrm();
        void Refreash();
        JToken GetCerts();
        JToken MakeLabel(JToken obj);
    }
    public class AKHelper : IAKHelper
    {
        AKCrm ak;
        //认证情况列表
        JToken certlist;
        DateTime checkPoint;
        public AKHelper()
        {
            checkPoint = DateTime.Now;
            ak = new AKCrm("17091923541", "duan1212");
            var certurl = ak.host + $"/api/v2/custom_fields/contact/by_group?user_token={ak.token}&version_code=9.9.9&device=open_api";
            var certs = ak.GET(certurl);
            var otherinfo = certs["data"]["custom_field_groups"].Where(c => c["label"].ToString() == "其他信息").First()["custom_fields"];
            certlist = otherinfo.Where(c => c["label"].ToString() == "获得认证情况").First()["input_field_options"]["collection_options"];       
        }

        public AKCrm GetAKCrm()
        {
            var check = DateTime.Now - checkPoint;
            if (check.TotalMinutes > 5)
            {
                Refreash();
            }
            return ak;
        }
        public void Refreash()
        {
            ak = new AKCrm("17091923541", "duan1212");
        }
        public JToken GetCerts()
        {
            return certlist;
        }
        /// <summary>
        /// 单个客户打标签的方法
        /// </summary>
        /// <param name="obj">一个数据字典, cu是账户id, la 是标签id数组,也可以是单个标签的整数型id</param>
        /// <returns>返回值直接使用爱客的,如果成功 code为0</returns>
        public JToken MakeLabel(JToken obj)
        {
            var customerid = obj["cu"];
            var label = obj["la"];
            var Identity = $"?user_token={ak.token}&version_code=9.9.9&device=open_api";
            var url = ak.host + $"/api/v2/customers/{customerid}" + Identity;
            var labels = new { customer = new { labels = label } };
            var result = ak.PUT(labels, url);

            //给个客户id和标签id数组
            return result;
        }

 

    }
}
