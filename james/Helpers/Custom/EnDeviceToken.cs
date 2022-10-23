using james.Helpers.Custom.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom
{
    public class EnDeviceToken
    {
        public string TokenID { get; set; }
        public bool isNotificationFlag { get; set; }
    }
    public class NotificationMessageData
    {
        public int NotificationID { get; set; }
        public bool isNotificationFlag { get; set; }
        public int? NotificationType { get; set; }
        public long? ReferenceID { get; set; }
        public int MessageType { get; internal set; }
        public string SenderName { get; set; }
        public string SenderPhoto { get; set; }
        public EnAgoraToken agoraToken { get; internal set; }
    }
}
