using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.Custom.Api
{
    public class EnNotificatoinPayload
    {
        public int referenceId { get; set; }
        public int notificationType { get; set; }
        public string senderName { get; set; }
        public string senderPhoto { get; set; }
        public int messageType { get; set; }
        public EnAgoraToken agoraToken { get; internal set; }        
    }
}
