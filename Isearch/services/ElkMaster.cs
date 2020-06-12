using Nest;
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
    public class ElkMaster
    {
        public ElasticClient client;
        /// <summary>
        /// 构造函数，初始化客户端而已
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="host"></param>
        public ElkMaster(string user, string pass, string host)
        {
            client = new ElasticClient
                (
                new ConnectionSettings(
                    new Uri(host)
                    )
                .BasicAuthentication(user, pass)
                );
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
            //耗时12s
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

    }
}
