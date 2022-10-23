using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnSocialLogin
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public string photo { get; set; }
        public int source { get; set; }
    }
}
