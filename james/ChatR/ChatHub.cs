using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using james.Helpers.Custom;
using james.Helpers.Custom.Api;
using james.Models;
using james.Models.DB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace james.ChatR
{
    public class HubClients
    {
        public string username { get; set; }

        public string connectionId { get; set; }
    }
    public class ChatHub : Hub
    {
        public static List<HubClients> clients = new List<HubClients>();
        public static List<EnChatUser> ConnectedUsers = new List<EnChatUser>();
        ChatModel service;
        DbContextOptions<DBContext> dbOptions;
        IHubContext<ChatHub> _hubcontext;
        public ChatHub(DbContextOptions<DBContext> _dbOptions, IConfiguration _configuration, IHubContext<ChatHub> hubcontext)
        {
            this.dbOptions = _dbOptions;
            this._hubcontext = hubcontext;
            service = new ChatModel(this.dbOptions, _configuration, this._hubcontext);

        }
        public void Connect(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var id = Context.ConnectionId;
                var client = clients.Where(x => x.username == username && x.connectionId == id).FirstOrDefault();
                if (client == null)
                {
                    clients.Add(new HubClients { username = username, connectionId = id });
                }
                Clients.Caller.SendAsync("onConnected", id);
            }
        }

        public void ConnectChat(string _empId)
        {
            int empId = Convert.ToInt32(_empId);
            var id = Context.ConnectionId;
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new EnChatUser { ConnectionId = id, EmpId = empId });
                //   ConnectedUsers.Add(new UserDetail { ConnectionId = id, EmpId = empId });

                // send to caller
                Clients.Caller.SendAsync("onConnectedChat", id, empId, ConnectedUsers, new List<MessageDetail> { });
                //Clients.Caller.onConnectedChat(id, empId, ConnectedUsers, new List<MessageDetail> { });

                // send to all except caller client
                Clients.AllExcept(id).SendAsync("onNewUserConnected", id, empId);


            }

        }

        public void SendPrivateMessage(string senderIdStr, string toUserIdStr, string message, string MessengerThreadIdStr, short MessageType)
        {
            int senderId = Convert.ToInt32(senderIdStr);
            int toUserId = Convert.ToInt32(toUserIdStr);
            int MessengerThreadId = Convert.ToInt32(MessengerThreadIdStr);
                   // MessageType = 1 ? Text Message 
            // MessageType = 2 ? Attachment 


            string fromUserId = Context.ConnectionId;

            //  var toUser = ConnectedUsers.Where(x => x.EmpId == toUserId).ToList();
            //    var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);
            var _temp = ConnectedUsers.Where(x => x.EmpId == toUserId).Select(c => c.ConnectionId).ToList();
            var result = service.SaveEmployeeChat(senderId, toUserId, message, MessageType);

            var empList = new List<int> { senderId, toUserId };
            if (result)
            {

                string photo = string.Empty;
                using (DBContext db = new DBContext(this.dbOptions))
                {
                    int _userid = senderId;

                    var u = db.users.Where(x => x.id == _userid).Select(x => new { x.photo }).FirstOrDefault();
                    photo = u.photo;
                }


                //var toUserIdInt = Convert.ToInt32(toUserId);
                //var senderIdInt = Convert.ToInt32(senderId);
                //using (DBContext db = new DBContext(this.dbOptions))
                //{
                //        if (_temp.Count > 0)
                //        {
                //            Clients.Clients(_temp).SendAsync("PrivateMessageSendByUser", toUserId, senderId, message, MessageType, _temp, photo);

                //        }
                    
                //}


            }

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            var item = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (item != null)
            {
                var id = Context.ConnectionId;
                ConnectedUsers.Remove(item);


                if (!ConnectedUsers.Any(x => x.EmpId == item.EmpId))
                    Clients.All.SendAsync("onUserDisconnected", id, item.EmpId);

            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
