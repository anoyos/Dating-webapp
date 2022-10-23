using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnDiary
    {
        public int userId { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string photo { get; set; }
    }
}
