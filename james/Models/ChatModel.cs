using james.ChatR;
using james.Helpers.Custom;
using james.Helpers.Custom.Api;
using james.Helpers.General;
using james.Models.DB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models
{
    public class ChatModel
    {
        DbContextOptions<DBContext> dbOptions;
        public static string constr;
        IHubContext<ChatHub> _hubcontext;
        public ChatModel(DbContextOptions<DBContext> _dbOptions, IConfiguration _configuration, IHubContext<ChatHub> hubcontext)
        {

            constr = _configuration.GetConnectionString("DefaultConnection");
            this.dbOptions = _dbOptions;
            this._hubcontext = hubcontext;
        }
        public bool SaveEmployeeChat(int from_UserId, int to_UserId, string message, short MessageType = 1)
        {
            using (DBContext db = new DBContext(this.dbOptions))
            {
                var chatThreadId = db.chatThreads.Where(x => (x.user1Id == from_UserId || x.user1Id == to_UserId) && (x.user2Id == from_UserId || x.user2Id == to_UserId)).Select(x => x.id).FirstOrDefault();
                if (chatThreadId != 0)
                {
                    var cg = db.chatThreads.Where(x => x.id == chatThreadId).FirstOrDefault();
                    cg.last_message_timestamp = DateTime.Now;
                    cg.last_message = message;
                    db.SaveChanges();
                }
                else
                {
                    var cg = db.chatThreads.Add(new ChatThread
                    {
                        last_message_timestamp = DateTime.Now,
                        last_message = message,
                        user1Id = from_UserId,
                        user2Id = to_UserId,
                    });
                    db.SaveChanges();
                    chatThreadId = cg.Entity.id;
                }
                db.chats.Add(new james.Models.DB.Chat { chatThreadId = chatThreadId, message = message, messageType = MessageType, timestamp = DateTime.Now, senderId = from_UserId, });
                db.SaveChanges();
                var cgm = db.chatThreads.Where(x => x.id == chatThreadId).FirstOrDefault();
                if (cgm != null)
                {
                    if (from_UserId == cgm.user1Id)
                    {
                        cgm.user2_unread += 1;
                    }
                    else
                    {
                        cgm.user1_unread += 1;
                    }
                    db.SaveChanges();
                }
                NotificationModel n = new NotificationModel();
                var from = db.users.Where(x => x.id == from_UserId).FirstOrDefault();
                var tokens = db.firebaseTokens.Where(x => x.userId == to_UserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();
                n.PushNotificationToAndroid(tokens, "New Message", message, new Helpers.Custom.Api.EnNotificatoinPayload
                {
                    referenceId = from_UserId,
                    notificationType = (int)NotificationTypeEnum.NewMessage,
                    messageType = 1,
                    senderName = from.name,
                    senderPhoto = from.photo
                });

                var _temp = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == to_UserId).Select(c => c.ConnectionId).ToList();
                this._hubcontext.Clients.Clients(_temp).SendAsync("PrivateMessageSendByUser", to_UserId, from_UserId, message, MessageType, _temp, from.photo);
            }
            return true;
        }


        public List<EmployeeForChat> GetEmployeeListForChat(int empId, string q)
        {
            using (DBContext db = new DBContext(this.dbOptions))
            {
                var employeeList = new List<EmployeeForChat>();

                employeeList = db.chatThreads.Where(x => x.user1Id == empId || x.user2Id == empId).Select(x => new EmployeeForChat
                {
                    threadId=x.id,
                    EmpId = x.user1Id == empId ? x.user2Id : x.user1Id,
                    last_message = x.last_message,
                    message_type = x.message_type,
                    last_message_timestamp = x.last_message_timestamp,
                    Name = x.user1Id == empId ? x.user2.name : x.user1.name,
                    photo = x.user1Id == empId ? x.user2.photo : x.user1.photo,
                    UnReadCnt = x.user1Id == empId ? x.user1_unread : x.user2_unread,

                }).OrderByDescending(x=>x.last_message_timestamp).ToList();
                return employeeList;
            }
        }
        public List<EnMessaging> GetEmployeeChat(int empId, int loginUserId)
        {
            using (DBContext db = new DBContext(this.dbOptions))
            {

                var chatThreadId = db.chatThreads.Where(x => (x.user1Id == empId || x.user1Id == loginUserId) && (x.user2Id == empId || x.user2Id == loginUserId)).Select(x => x.id).FirstOrDefault();
                return db.chats.Where(x => x.chatThreadId == chatThreadId).Select(msg => new EnMessaging
                {
                    Id = msg.id,
                    chatThreadId = msg.chatThreadId,
                    SenderId = msg.senderId,
                    Name = msg.sender.name,
                    Message = msg.message,
                    TimeStamp = msg.timestamp,
                    MessageType = msg.messageType,
                    Photo = msg.sender.photo,
                }).ToList();
            }

           
        }

    }
}
