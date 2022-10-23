using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnRating
    {
        public int fromUserId { get; set; }
        public int toUserId { get; set; }
        public double rate { get; set; }
        public string review { get; set; }
    }
    public class EnRatingList
    {
        public string name { get; set; }
        public string photo { get; set; }
        public double rate { get; set; }
        public string review { get; set; }
        public DateTime timestamp { get; set; }
    }
}
