using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnFilter
    {

        public int myId { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string name { get; set; }
        public int? startAge { get; set; }
        public int? EndAge { get; set; }
        public string city { get; set; }
    
        public bool subscribedProfile { get; set; }
        public List<int> lookingforRelation { get; set; }
        public List<int> relationStatus { get; set; }
        public int? lookingForGender { get; set; }
        public string profession { get; set; }
        
        public string location { get; set; }
        public int? children { get; set; }
        public int? smoke { get; set; }
        public int? sexualOrientation { get; set; }
        public int? bodyArt { get; set; }
        public int? religion { get; set; }
    
    }
}
