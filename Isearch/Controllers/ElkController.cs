using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Isearch.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Isearch.Controllers
{
    public class ElkController : Controller
    {
        private ElasticClient client = null;

        public ILogger Logger { get; }
        public IAKHelper AKHelper { get; }
        public AKCrm ak;

        public ElkController(ILogger logger, IAKHelper aKHelper)
        {
            client = new ElasticClient(
                new ConnectionSettings(new Uri("http://ip.lxgreg.cn:19200")).BasicAuthentication("elastic", "duan1212")
                );
            Logger = logger;
            AKHelper = aKHelper;
            ak = AKHelper.GetAKCrm();
        }
        [Authorize(Roles = "CrmDeveloper")]
        public IActionResult GetAllData(int total, string model)
        {
            if (total == 0)
            {
                total = 5;
            }
            string user = User.Identity.Name;
            var data = ElkData(total, model);
            return Json(data);
        }

        [HttpPost]
        public IActionResult checkurl()
        {
            var obj = new
            {
                msg_signature = "111108bb8e6dbce3c9671d6fdb69d15066227608",
                timeStamp = "1783610513",
                nonce = "123456",
                encrypt = "1ojQf0NSvw2WPvW7LijxS8UvISr8pdDP+rXpPbcLGOmIBNbWetRg7IP0vdhVgkVwSoZBJeQwY2zhROsJq/HJ+q6tp1qhl9L1+ccC9ZjKs1wV5bmA9NoAWQiZ+7MpzQVq+j74rJQljdVyBdI/dGOvsnBSCxCVW0ISWX0vn9lYTuuHSoaxwCGylH9xRhYHL9bRDskBc7bO0FseHQQasdfghjkl"
            };
            return Json(obj);
        }

        #region 旧api,准备弃用




        [HttpPost]
        [Authorize(Roles = "CrmDeveloper")]
        public IActionResult Label([FromBody]object target)
        {
            try
            {
                var targets = JsonConvert.SerializeObject(target);
                var crm = new AKCrm("17091923541", "duan1212");
                var result = crm.Label(targets);
                string user = User.Identity.Name;
                var logobj = new
                {
                    用户 = user,
                    系统 = "CRM",
                    模型 = "客户",
                    操作 = $"[旧方法]对客户打标签,data:{target}",
                    时间 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800")
                };
                Logger.Logger(logobj);
                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        //获取认证情况
        private static JToken getCertList(AKCrm ak)
        {
            var certurl = ak.host + $"/api/v2/custom_fields/contact/by_group?user_token={ak.token}&version_code=9.9.9&device=open_api";
            var certs = ak.GET(certurl);
            var otherinfo = certs["data"]["custom_field_groups"].Where(c => c["label"].ToString() == "其他信息").First()["custom_fields"];
            var certlist = otherinfo.Where(c => c["label"].ToString() == "获得认证情况").First()["input_field_options"]["collection_options"];
            return certlist;
        }

        [HttpPost]
        [Authorize(Roles = "CrmDeveloper")]
        public IActionResult ContactInsert([FromBody]object contact)
        {
            //转型,然后拿对应的客户名称查询客户id
            var ak = new AKCrm("17091923541", "duan1212");
            var obj = JsonConvert.SerializeObject(contact);
            var test = JObject.Parse(obj);
            //var jobj = JObject.Parse(contact);
            var customername = test["contact"]["对应客户"].ToString();
            var contactname = test["contact"]["name"].ToString();
            string[] fields = new string[] { "id", "name", "created_at" };
            var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("cufull").Size(19999).Source(c => c.Includes(f => f.Fields(fields)))
            ).Documents;
            //如果elk有重名的还得去最新的那个
            var result = cus.Where(c => c["name"] == customername).OrderByDescending(c => DateTime.Parse(c["created_at"]));

            //既然拿到了客户id,就得给获得认证情况字段判断id了
            var certurl = ak.host + $"/api/v2/custom_fields/contact/by_group?user_token={ak.token}&version_code=9.9.9&device=open_api";
            var certs = ak.GET(certurl);

            var otherinfo = certs["data"]["custom_field_groups"].Where(c => c["label"].ToString() == "其他信息").First()["custom_fields"];
            var certlist = otherinfo.Where(c => c["label"].ToString() == "获得认证情况").First()["input_field_options"]["collection_options"];
            //var contactcert = contact["contact"];
            var cert = certlist.Where(c => c["label"].ToString() == test["contact"]["text_asset_485920_display"].ToString());
            if (cert.Count() > 0)
            {
                test["contact"]["text_asset_485920"] = cert.First()["value"];
            }
            var name = User.Identity.Name;
            var logobj = new
            {
                用户 = name,
                系统 = "CRM",
                模型 = "联系人",
                操作 = "",
                时间 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800")
            };

            //有客户就得查是不是已经有这个联系人
            if (result.Count() > 0)
            {
                var cuid = result.First()["id"];
                test["contact"]["customer_id"] = cuid;
                var akurl = ak.host + $"/api/v2/contacts/simplest?customer_id={cuid}&user_token={ak.token}&version_code=9.9.9&device=open_api";
                var contacts = ak.GET(akurl);
                JArray contactlist = contacts["data"]["contacts"] as JArray;
                var contactquery = contactlist.Where(c => c["name"].ToString() == contactname);
                if (contactquery.Count() > 0)
                {
                    var contactid = contactquery.FirstOrDefault()["id"];
                    var updateresult = contactupdate(contactid.ToString(), test, ak);
                    logobj = new
                    {
                        用户 = name,
                        系统 = "CRM",
                        模型 = "联系人",
                        操作 = $"更新:{test["contact"]}[旧方法]",
                        时间 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800")
                    };
                    Logger.Logger(logobj);
                    return Json(updateresult);
                }
                else
                {
                    var addresult = contactadd(test, ak);

                    logobj = new
                    {
                        用户 = name,
                        系统 = "CRM",
                        模型 = "联系人",
                        操作 = $"创建:{test["contact"]}[旧方法]",
                        时间 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800")
                    };
                    Logger.Logger(logobj);
                    return Json(addresult);
                }
            }
            //拿到客户id,尝试推送联系人到客户
            //先判断客户下面有没有这个联系人,没有就新增,有就得更新..

            return Json(new { error = "错误,没有找到客户", contact });
        }

        JObject contactupdate(string cid, JObject contact, AKCrm ak)
        {
            var url = ak.host + $"/api/v2/contacts/{cid}?user_token={ak.token}&version_code=9.9.9&device=open_api";
            var result = ak.PUT(contact, url);
            return result;
        }

        JObject contactadd(JObject contact, AKCrm ak)
        {
            var url = ak.host + $"/api/v2/contacts?user_token={ak.token}&version_code=9.9.9&device=open_api";
            var result = ak.Post(contact, url);
            return result;
        }



        /// <summary>
        /// python 使用方式
        /// url = 'http://localhost:57837/elk/test'
        ///headers={'Content-Type': 'application/json'}
        ///obj = ['name']
        ///ss= requests.post(url, headers=headers,data=json.dumps(obj).encode())
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "CrmDeveloper")]
        public IActionResult GetData([FromBody]string[] fields, string model, int total)
        {
            if (total == 0) { total = 5; }
            var data = ElkData(total, model, fields);
            try
            {
                string user = User.Identity.Name;
                var logobj = new
                {
                    用户 = user,
                    系统 = "CRM",
                    模型 = model,
                    操作 = $"读取:{data.Count}条数据，涉及字段{JsonConvert.SerializeObject(fields)}",
                    时间 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800")
                };
                Logger.Logger(logobj);
            }
            catch (Exception ex)
            {

                return Json(ex);
            }
            return Json(data);
        }

        [HttpGet]
        [Authorize(Roles = "CrmDeveloper")]
        public string Tri(string customer)
        {
            AKCrm ak = new AKCrm("17091923541", "duan1212");
            try
            {
                string[] status = { "培训", "签代理协议", "成交" };
                var cuid = CheckCustomer(status, ak, customer);

                if (cuid != null)
                {
                    var result = ak.Trained(cuid);
                    string user = User.Identity.Name;
                    var logobj = new
                    {
                        用户 = user,
                        系统 = "CRM",
                        模型 = "客户",
                        操作 = $"修改渠道阶段为‘培训’[旧方法]",
                        时间 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800")
                    };
                    Logger.Logger(logobj);
                    return result;
                }
                return "客户未找到或者有重复";

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }



        #endregion



        [HttpGet]
        [Authorize(Roles = "CrmDeveloper")]
        public string Tri2(string customer)
        {
            //            操作 = $"修改客户:{query["id"]}的渠道阶段为‘培训’",
            string[] status = { "培训", "签代理协议", "成交" };
            var res = new { user = User.Identity.Name, status = 100, system = "CRM", model = "Customer", action = $"推进客户:{customer}-渠道阶段至培训", action_time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800") };
            var res_json = JObject.FromObject(res);
            try
            {
                //AKCrm ak = new AKCrm("17091923541", "duan1212");
                var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("cufull").QueryOnQueryString("name.keyword :" + customer)).Documents;
                if (cus.Count == 0)
                {
                    res_json.Add("reason", "customer is not exist");
                }
                else
                {
                    var fcustomer = JObject.FromObject(cus.OrderBy(c => c["created_at"]).First());
                    if (status.Contains(fcustomer["status_mapped"].ToString()))
                    {
                        //不符合修改条件
                        res_json.Add("reason", $"status wrong:{fcustomer["status_mapped"]}");
                    }
                    else
                    {
                        var result = ak.Trained(fcustomer["id"].ToString());
                        if (result != "OK")
                        {
                            res_json.Add("reason", result);
                        }
                        else
                        {
                            res_json["status"] = 0;
                        }

                    }
                }
            }
            catch (Exception ex) {
                res_json.Add("reason", JObject.FromObject(ex));
            }
            Logger.Logger(res_json);
            return JsonConvert.SerializeObject(res_json);




        }

        [Authorize(Roles = "CrmDeveloper")]
        public string GetCustomer(string queryStr)
        {
            var res = new { user = User.Identity.Name, status = 100, system = "CRM", model = "Customer", action = $"查询客户", action_time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800") };
            var res_json = JObject.FromObject(res);
            try
            {
                //预定好的返回值字段
                string[] fields = { "id", "name", "user.name", "user.id", "owned_department.name", "owned_department.id", "labels" };
                var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("cufull").QueryOnQueryString(queryStr).Size(20000)
                    .Source(c => c.Includes(f => f.Fields(fields)))).Documents;
                res_json["status"] = 0;
                res_json.Add("count", cus.Count);
                Logger.Logger(res_json);
                res_json.Add("data", JToken.FromObject(cus));

            }
            catch(Exception ex)
            {
                res_json.Add("error", ex.Message); 
                Logger.Logger(res_json);
            }


            //Logger.Logger(res_json);
            return JsonConvert.SerializeObject(res_json);
        }

        [Authorize(Roles = "CrmDeveloper")]
        public string GetContract(string queryStr)
        {
            var res = new { user = User.Identity.Name, status = 100, system = "CRM", model = "Contract", action = $"查询合同", action_time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800") };
            var res_json = JObject.FromObject(res);
            try
            {
                //预定好的返回值字段
                string[] fields = {
                    "sn", 
                    "customer.name", 
                    "customer.id",
                    "text_asset_c55e5f",
                    "sign_date",
                    "owned_department.name",
                    "user.name",
                };
                var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("contract001").QueryOnQueryString(queryStr).Size(20000)
                    .Source(c => c.Includes(f => f.Fields(fields)))).Documents;
                res_json["status"] = 0;
                res_json.Add("count", cus.Count);
                Logger.Logger(res_json);
                res_json.Add("data", JToken.FromObject(cus));

            }
            catch (Exception ex)
            {
                res_json.Add("error", ex.Message);
                Logger.Logger(res_json);
            }


            //Logger.Logger(res_json);
            return JsonConvert.SerializeObject(res_json);
        }



        [Authorize(Roles = "CrmDeveloper")]
        public string GetOpportuniy(string queryStr)
        {
            var res = new { user = User.Identity.Name, status = 100, system = "CRM", model = "Opportunity", action = $"查询商机", action_time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800") };
            var res_json = JObject.FromObject(res);
            try
            {
                //预定好的返回值字段
                string[] fields = {
                    "id",
                    "owned_department",
                    "product_assets",
                    "title",
                    "stage_mapped",
                    "created_at",
                    "user.name",
                    "customer.name",
                    "updated_at",
                    "expect_sign_date",
                    "text_asset_6f4e3b_display",
                };
                var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("opfull1").QueryOnQueryString(queryStr).Size(20000)
                    .Source(c => c.Includes(f => f.Fields(fields)))).Documents;
                res_json["status"] = 0;
                res_json.Add("count", cus.Count);
                Logger.Logger(res_json);
                res_json.Add("data", JToken.FromObject(cus));

            }
            catch (Exception ex)
            {
                res_json.Add("error", ex.Message);
                Logger.Logger(res_json);
            }


            //Logger.Logger(res_json);
            return JsonConvert.SerializeObject(res_json);
        }





        [HttpPost]
        [Authorize(Roles = "CrmDeveloper")]
        public string Contact([FromBody]object contact)
        {
            var res = new { user = User.Identity.Name, status = 100, system = "CRM", model = "Contact", action = $"推送联系人", body = contact, action_time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800") };

            var res_json = JObject.FromObject(res);
            try
            {
                var Jcontact = JObject.FromObject(contact);
                var customername = Jcontact["contact"]["对应客户"].ToString();
                var contactname = Jcontact["contact"]["name"].ToString();
                //使用客户名称精确查找唯一客户的id
                var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("cufull").QueryOnQueryString("name.keyword :" + customername)).Documents;
                if (cus.Count == 0)
                {
                    res_json.Add("reason", "customer is not exist");
                }
                else
                {
                    var fcustomer = JObject.FromObject(cus.OrderBy(c => c["created_at"]).First());
                    var fcontacts = fcustomer["contacts"];

                    //根据认证字符串获取该字段的id
                    var certlist = AKHelper.GetCerts();
                    var cert = certlist.Where(c => c["label"].ToString() == Jcontact["contact"]["text_asset_485920_display"].ToString());
                    if (cert.Count() == 0)
                    {
                        res_json.Add("reason", "认证情况字段字段未能匹配成功");
                    }
                    else
                    {
                        Jcontact["contact"]["text_asset_485920"] = cert.First()["value"];
                        Jcontact["contact"]["customer_id"] = fcustomer["id"];
                        var Jphone = Jcontact["contact"]["address_attributes"]["phone"].ToString();
                        var finduser = fcontacts.Where(
                            c => c["name"].ToString() == Jcontact["contact"]["name"].ToString()
                            && c["address"]["phone"].ToString() == Jphone
                            );

                        res_json["body"] = Jcontact;
                        JObject result;
                        if (finduser.Count() == 1)
                        {
                            //更新
                            var cid = finduser.First()["id"];
                            var url = ak.host + $"/api/v2/contacts/{cid}?user_token={ak.token}&version_code=9.9.9&device=open_api";
                            result = ak.PUT(Jcontact, url);
                            //这里有可能失败，可能elk里有数据但实际上crm已经删除了，所以加一次post
                            if (result["code"].ToString() != "0")
                            {//创建联系人
                                var akurl = ak.host + $"/api/v2/contacts?user_token={ak.token}&version_code=9.9.9&device=open_api";
                                result = ak.Post(Jcontact, akurl);
                            }
                        }
                        else
                        {
                            //创建联系人
                            var akurl = ak.host + $"/api/v2/contacts?user_token={ak.token}&version_code=9.9.9&device=open_api";
                            result = ak.Post(Jcontact, akurl);
                        }
                        res_json["status"] = result["code"];
                        if (res_json["status"].ToString() != "0")
                        {
                            res_json.Add("reason", result["message"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                res_json.Add("reason", ex.Message);
            }
            Logger.Logger(res_json);
            return JsonConvert.SerializeObject(res_json);
        }

        [HttpPost]
        [Authorize(Roles = "CrmDeveloper")]
        public string LabelUp([FromBody]object obj)
        {
         
                var res = new { user = User.Identity.Name, status = 100, system = "CRM", model = "Customer", action = $"打标签", body = obj, action_time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss+0800") };
                var res_json = JObject.FromObject(res);
        
                try
                {
                    var jobj = JObject.FromObject(obj);
                    var result = AKHelper.MakeLabel(jobj);
                    if (result["code"] == null)
                    {
                        res_json.Add("error", result);
                    }
                    else
                    {
                        if (result["code"].ToString() == "0")
                        {
                            res_json["status"] = 0;
                            res_json.Add("currentLabel", result["data"]["labels"]);
                        }
                        else
                        {
                            res_json.Add("error", result["message"]);
                        }
                    }

                }
                catch (Exception ex)
                {
                    res_json.Add("error", ex.Message);
                }

                Logger.Logger(res_json);
                return JsonConvert.SerializeObject(res_json);
       
        }
        


        ///
        [AllowAnonymous]
        public IActionResult Index()
        {
            return Json("回头这里放说明");
        }

        /// <summary>
        /// 基础获取elk数据的方式
        /// </summary>
        /// <param name="total">获取数量，比如很少的时候用，填0则全部获取</param>
        /// <param name="Index">elk的模型</param>
        /// <param name="fields">要返回的字段</param>
        /// <returns></returns>
        public JArray ElkData(int total, string Index, Fields fields)
        {
            //判断取行数，如果是0就判断全取下面的方法只是快速获取总数，否则按照实际要求取值
            if (total == 0)
            {
                var search = client.Search<dynamic>(sr => sr.Index(Index).Source(false));
                total = Convert.ToInt32(search.Total);
                // timecount = TimeCount(ref timestap);
            }
            //设置返回的字段        
            //.source 实现返回特定字段 配置一个字符串数组fieldes，用lambda表达式带进去，例如  .Source(c=>c.Includes(f=>f.Fields(fields)))，支持排除字段.Excludes 用法同Includes
            //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/source-filtering-usage.html
            var result = client.Search<dynamic>(
            s => s.Index(Index)
                .Size(total)
                .Source(c => c.Includes(f => f.Fields(fields)))
                );
            JArray ss = JArray.FromObject(result.Documents.ToList());
            return ss;
        }


        /// <summary>
        /// 重载方法，获取所有字段
        /// </summary>
        /// <param name="total">获取数量，比如很少的时候用，填0则全部获取</param>
        /// <param name="Index">elk的模型</param>
        /// <returns></returns>
        public JArray ElkData(int total, string Index)
        {
            //判断取行数，如果是0就判断全取下面的方法只是快速获取总数，否则按照实际要求取值
            if (total == 0)
            {
                var search = client.Search<dynamic>(sr => sr.Index(Index).Source(false));
                total = Convert.ToInt32(search.Total);
                // timecount = TimeCount(ref timestap);
            }
            //设置返回的字段        
            //.source 实现返回特定字段 配置一个字符串数组fieldes，用lambda表达式带进去，例如  .Source(c=>c.Includes(f=>f.Fields(fields)))，支持排除字段.Excludes 用法同Includes
            //https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/source-filtering-usage.html
            var result = client.Search<dynamic>(
            s => s.Index(Index)
                .Size(total)
                );
            JArray ss = JArray.FromObject(result.Documents.ToList());
            return ss;
        }



        //检查客户阶段
        private string CheckCustomer(string[] status, AKCrm ak, string keyword)
        {
            var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("cufull").QueryOnQueryString("name.keyword :" + keyword)).Documents;            
            if (cus.Count > 0)
            {
                var result = JObject.FromObject(cus.OrderBy(c => c["created_at"]).First());
                var custa = result["status_mapped"];
                if (!status.Contains(custa.ToString()))
                {
                    return result["id"].ToString();
                }
            }
            return null;
        }




    }
}