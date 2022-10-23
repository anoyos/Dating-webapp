using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnStreaming
    {
        public string title { get; set; }
        public string banner { get; set; }
        public string category { get; set; }
        public int userId { get; set; }
        
    }
    public class EnStreamingDetail
    {
        public int id { get; set; }
        public string token { get; set; }
        public string channel { get; set; }
        public string title { get; set; }
        public string banner { get; set; }
        public string category { get; set; }
        public int userId { get; set; }
        public string photo { get;  set; }
        public string username { get;  set; }
        public bool likeByMe { get;  set; }
    }
    public class EnStreamAddComment
    {
        public int streamId { get; set; }
        public int userId { get; set; }
        public string comment { get; set; }
    }
    public class EnStreamAddLike
    {
        public int streamId { get; set; }
        public int userId { get; set; }
        public bool isLike { get; set; }
    }
    public class EnStreamAddSubComment
    {
        public int streamId { get; set; }
        public int userId { get; set; }
        public int commentId { get; set; }
        public string comment { get; set; }
    }
    public class EnStreamComment
    {
        public int id { get; set; }
        public DateTime timestamp { get; set; }
        public string comment { get; set; }
        public string name { get;  set; }
        public string photo { get;  set; }
    }
    public class EnStreamStats
    {
        public int likes { get; set; }
        public int views { get; set; }       
    }
    public class EnStreamList
    {
        public int id { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string banner { get; set; }
    }
}