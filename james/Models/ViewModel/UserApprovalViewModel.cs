using james.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models.ViewModel
{
    public class UserApprovalViewModel
    {
        public bool userAutoApprove { get; set; }
        public List<User> users { get; set; }
    }
}
