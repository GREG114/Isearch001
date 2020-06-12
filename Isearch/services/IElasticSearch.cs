using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
