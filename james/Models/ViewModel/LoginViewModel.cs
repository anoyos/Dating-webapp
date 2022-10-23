using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models.ViewModel
{
    public class LoginViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int source { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public  bool error { get; set; }
        public string message { get; set; }
    }
    public class ForgotViewModel
    {
        public string email { get; set; }     
        public bool error { get; set; }
        public string message { get; set; }
    }
    public class ForgotCodeViewModel
    {

        public string email { get; set; }
        public string password { get; set; }
        public string code { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
        public string uid { get;  set; }
    }
}
