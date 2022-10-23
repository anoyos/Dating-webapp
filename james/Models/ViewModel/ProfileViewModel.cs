using james.Helpers.Custom.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models.ViewModel
{
    public class ProfileViewModel
    {
        public EnProfile profile { get;  set; }
        public bool me { get; internal set; }
    }
}
