using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NLog.RestApiTargetServer.Controllers
{
    public class Test
    {
        public string Level { get; set; }
        public string Timestamp { get; set; }
        public string Message { get; set; }
        public string goal { get; set; }
    }

    public class HelloController : ApiController
    {
        [HttpPost]
        public string Indexer([FromBodyAttribute]Test test)
        {
            var body = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject(body);
            var headers = HttpContext.Current.Request.Headers.AllKeys.ToList();
            var values = HttpContext.Current.Request.Headers.Get("CustomHeader");
            return "hello world";
        }
    }
}
