using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Isearch.services
{
    public class req
    {
        public static JObject Get(string url)
        {
            var req = WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "application/json;charset=utf-8";
            req.Credentials = new NetworkCredential("elastic", "duan1212");
            var res = req.GetResponse();
            var str = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd().ToString();
            JObject result = JsonConvert.DeserializeObject<JObject>(str);
            req.Abort();
            return result;
        }

        public static JObject Post(object obj, string url)
        {
            var req = WebRequest.Create(url);
            req.Credentials = new NetworkCredential("elastic", "duan1212");
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
    }
}
