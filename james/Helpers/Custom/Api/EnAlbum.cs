using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnAlbum
    {
        public int id { get; set; }
       public int userId { get; set; }
        public string name { get; set; }
        public List<string> images { get; set; }
        public bool isPrivate { get; internal set; }
    }
    public class EnAlbumImgObj
    {
        public int id { get; set; }
        public List<string> images { get; set; }
    }
}
