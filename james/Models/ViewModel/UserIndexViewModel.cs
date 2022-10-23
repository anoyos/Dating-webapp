using james.Helpers.Custom.Api;
using james.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models.ViewModel
{
    public class UserIndexViewModel
    {
        public List<EnUser> users { get; set; }
        public List<DDL> ddls { get; set; }
        public EnFilter filter { get;  set; }
        public List<EnStory> stories { get;  set; }
        public List<EnReportUserOptionList> reportList { get;  set; }
    }
}
