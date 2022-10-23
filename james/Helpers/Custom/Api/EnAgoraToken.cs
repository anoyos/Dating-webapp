using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnAgoraToken
    {
        public string token { get; set; }
        public string channel { get; set; }
        public int callId { get; set; }
        public bool isVideo { get; set; }
    }
}
