using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnStory
    {
        public int userId { get; set; }
        public List<EnStoryPhoto> photos { get; set; }
        public string name { get;  set; }
        public string userPhoto { get; set; }
        public long timestamp { get; set; }
    }
    public  class EnStoryPhoto
    {
        public string content { get; set; }
        public string photo { get; set; }
    }
}
