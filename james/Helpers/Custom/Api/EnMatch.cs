using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnMatch
    {
        public List<EnUser> matches { get; set; }
        public List<EnUser> likes { get; set; }
        public List<EnUser> likesMe { get; set; }
    }
}
