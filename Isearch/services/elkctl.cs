using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Isearch.services
{
    public class elkctl
    {
        public elkctl()
        {
            
        }
        public string query(string url)
        {
            string result="";
            try {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd().ToString();
                result = responseString;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
       
            return result;
        }
        public string postObj(string url,object obj)
        {
            string result = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                var objstr = JsonConvert.SerializeObject(obj);
                request.ContentType = "application/json;charset=UTF-8";
                byte[] byteData = Encoding.UTF8.GetBytes(objstr);
                int length = byteData.Length;
                request.ContentLength = length;
                Stream writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd().ToString();
                //   var obj1 = JObject.Parse(responseString);
                result = responseString;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
        public string putwithid(string url, object obj)
        {
            string result = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
                var objstr = JsonConvert.SerializeObject(obj);
                request.ContentType = "application/json;charset=UTF-8";
                byte[] byteData = Encoding.UTF8.GetBytes(objstr);
                int length = byteData.Length;
                request.ContentLength = length;
                Stream writer = request.GetRequestStream();
                writer.Write(byteData, 0, length);
                writer.Close();
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd().ToString();
                //   var obj1 = JObject.Parse(responseString);
                result = responseString;
                }catch(Exception ex)
                {
                result = ex.Message;
                }
       
            return result;
        }
    }
}
