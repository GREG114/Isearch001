using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Isearch.Controllers
{
    public class AKCrm
    {
        public string host = @"https://dingtalk.e.ikcrm.com";
        public string device = "open_api";
        public string token = null;
        public string essential = "";
        public JObject Post(object obj, string url)
        {
            var req = WebRequest.Create(url);
            req.Method = "POST";
            var objstr = JsonConvert.SerializeObject(obj);
            req.ContentType = "application/json;charset=utf-8";
            var byteData = Encoding.UTF8.GetBytes(objstr);
            int len = byteData.Length;
            req.ContentLength = len;
            var writer = req.GetRequestStream();
            writer.Write(byteData, 0, len);
            writer.Close();
            var res = req.GetResponse();
            var str = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd().ToString();
            JObject result = JsonConvert.DeserializeObject<JObject>(str);
            req.Abort();
            return result;

        }
        public JObject GET( string url)
        {
            var req = WebRequest.Create(url);
            req.Method = "GET";
            var res = req.GetResponse();
            var str = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd().ToString();
            JObject result = JsonConvert.DeserializeObject<JObject>(str);
            req.Abort();
            return result;

        }
        public JObject PUT(object obj, string url)
        {
            var req = WebRequest.Create(url);
            req.Method = "PUT";
            var objstr = JsonConvert.SerializeObject(obj);
            req.ContentType = "application/json;charset=utf-8";
            var byteData = Encoding.UTF8.GetBytes(objstr);
            int len = byteData.Length;
            req.ContentLength = len;
            var writer = req.GetRequestStream();
            writer.Write(byteData, 0, len);
            writer.Close();
            var res = req.GetResponse();
            var str = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd().ToString();
            JObject result = JsonConvert.DeserializeObject<JObject>(str);
            req.Abort();
            return result;

        }
        public AKCrm(string login, string password)
        {
            var url = host + $"/api/v2/auth/login";
            var obj = new { device, login, password };
            var result = Post(obj, url);
            token = result["data"]["user_token"].ToString();
            essential = $"?user_token={token}&version_code=9.9.9&device=open_api";
        }

        public string Trained(string id)
        {

            try
            {
                var customerid = id;
                //var label = obj["la"];
                var Identity = $"?user_token={token}&version_code=9.9.9&device=open_api";
                var url = host + $"/api/v2/customers/{customerid}" + Identity;
                var obj = new { customer = new { status = 10339020 } };
                //var labelstr = JsonConvert.SerializeObject(labels);
                //Console.WriteLine(labelstr);
                var result = PUT(obj, url);
                if (Convert.ToInt32(result["code"]) != 0)
                {
                    //  obj.Add("result", JObject.FromObject(result));
                    return JsonConvert.SerializeObject(result);
                }
                else
                {
                    return "OK";
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }

        }
        public string Label(string target)
        {
            var arr = JsonConvert.DeserializeObject<JArray>(target);
            foreach (JObject obj in arr)
            {
                try
                {
                    var customerid = obj["cu"];
                    var label = obj["la"];
                    var Identity = $"?user_token={token}&version_code=9.9.9&device=open_api";
                    var url = host + $"/api/v2/customers/{customerid}" + Identity;
                    var labels = new { customer = new { labels = label } };
                    //var labelstr = JsonConvert.SerializeObject(labels);
                    //Console.WriteLine(labelstr);
                    var result = PUT(labels, url);
                    if (Convert.ToInt32(result["code"]) != 0)
                    {
                        obj.Add("result", JObject.FromObject(result));
                    }
                    else
                    {
                        obj.Add("result", "ok");

                    }
                }
                catch (Exception ex)
                {
                    obj.Add("result", JObject.FromObject(ex));
                }
            }

            return JsonConvert.SerializeObject(arr);
        }
    }
}