using james.Helpers.Custom.Api;
using james.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models.ViewModel
{
    public class EditProfileViewModel
    {
        public EnRegisterEdit data { get; set; }
        public List<DDL> ddls { get;  set; }
    }
}
