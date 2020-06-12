using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isearch.services;
using Microsoft.AspNetCore.Mvc;
using Nest;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Isearch.Controllers
{
    public class APIController : Controller
    {
        public ElasticClient client;
        public APIController(IElastic elasticSearch)
        {
            client = elasticSearch.GetClient();
            
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Test()
        {//.QueryOnQueryString("name.keyword :" + keyword))
            var cus = client.Search<Dictionary<string, dynamic>>(s => s.Index("cufull").Size(3)).Documents;

            return Json(cus);
        }
    }
}
