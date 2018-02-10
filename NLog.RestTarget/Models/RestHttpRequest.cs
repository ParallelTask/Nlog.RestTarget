using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.RestTarget.Models
{
    public class RestHttpRequest
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public Object Body { get; set; } = new { };
    }
}
