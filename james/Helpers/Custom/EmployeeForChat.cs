using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom
{
    public class EmployeeForChat
    {
  
        public int EmpId { get; set; }
        public string Name { get; set; }
        public List<EnMessaging> Chat { get; set; }
        public string OnlineStatus { get; set; }

        public string photo { get; set; }
        public int UnReadCnt { get; set; }
        public int message_type { get; set; }
        public DateTime? last_message_timestamp { get; set; }
        public string last_message { get; set; }
        public int threadId { get;  set; }
    }
    public class EnMessaging
    {
        public long Id { get; set; }
        public int SenderId { get; set; }
        public int chatThreadId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public int MessageType { get; set; }
        public string Photo { get; set; }

    }
    public class MessageDetail
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public int messagetype { get; set; }
    }
    public class EnChatHeader
    {
        public string name { get; set; }
        public string photo { get; set; }
        public int id { get;  set; }
    }
}
