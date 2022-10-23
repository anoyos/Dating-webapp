using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom
{
    public class tblLogin
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string photo { get; set; }
        public Nullable<long> roleId { get; set; }      
    }

    public class GoogleLoginResp
    {
        public string sub { get; set; }
        public string picture { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
    }
}