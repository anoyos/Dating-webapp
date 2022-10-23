using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnChat
    {
        public int userId { get; set; }
        public string name { get; set; }
        public string photo { get; set; }
        public bool isOnline { get; set; }
        public DateTime? timeStamp { get; set; }
        public string lastMessage { get; set; }
        public int unreadCount { get; set; }
        public int? messageType { get; set; }
        public string attachment { get; set; }
        public string duration { get; set; }
    }
    public class EnChatMessage
    {
        public int id { get; set; }
        public int senderId { get; set; }
        public string senderName { get; set; }
        public string senderhoto { get; set; }           
        public DateTime? timeStamp { get; set; }
        public string message { get; set; }
        public int? messageType { get; set; }
        public string attachment { get; set; }
        public string duration { get; set; }
    }
    public class EnChatMessageSend
    {
        public int id { get; set; }
        public int fromId { get; set; }
        public int toId { get; set; }
        public string message { get; set; }
        public short type { get; set; }
     
    }
    public class EnNotificationFlag
    {
        public int userId { get; set; }
        public string device { get; set; }
        public bool isNotificationFlag { get; set; }
    }
}
