using james.ChatR;
using james.Helpers.Custom;
using james.Helpers.General;
using james.Models;
using james.Models.DB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Controllers
{
    public class MessengerController : BaseController
    {
        static Dictionary<string, string> dict = new Dictionary<string, string>();
        private readonly IWebHostEnvironment _hostingEnvironment;
        IConfiguration _configuration;
        ChatModel service;
        public MessengerController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, DbContextOptions<DBContext> _dbOptions, IHubContext<ChatHub> _hubcontext) : base(_dbOptions, hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this.dbOption = _dbOptions;
            service = new ChatModel(_dbOptions, configuration, _hubcontext);
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public string GetEmployeeListForChat()
        {
            var result = service.GetEmployeeListForChat(login.id, null);


            foreach (var item in result)
            {
                item.OnlineStatus = ChatHub.ConnectedUsers.Any(x => x.EmpId == item.EmpId) ? "online" : null;
            }
            return Common.Serialize(result);
        }
        public string MarkRead(int threadId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.chatThreads.Where(x => x.id == threadId).FirstOrDefault();
                if (e != null)
                {

                    if (e != null)
                    {
                        if (login.id == e.user1Id)
                        {
                            e.user2_unread += 1;
                        }
                        else
                        {
                            e.user1_unread += 1;
                        }
                        db.SaveChanges();
                    }
                    db.SaveChanges();

                }
                return Common.Serialize("success");
            }
        }

        public ActionResult ChatThreads(string q)
        {
            var result = service.GetEmployeeListForChat(login.id, q);
            foreach (var item in result)
            {
                item.OnlineStatus = ChatHub.ConnectedUsers.Any(x => x.EmpId == item.EmpId) ? "online" : null;
            }
            if (!string.IsNullOrEmpty(q))
            {
                result = result.Where(x => !string.IsNullOrWhiteSpace(x.last_message)).ToList();
                foreach (var item in result)
                {
                    item.last_message = item.last_message.Replace(q, "<span class='highlight'>" + q + "</span>");
                }
            }
            return View(result);
        }
        public ActionResult _chatMessage(int EmpId, int? threadId)
        {
            MarkRead(threadId ?? 0);
            using (DBContext db = new DBContext(this.dbOption))
            {
                ViewBag.Header = db.users.Where(x => x.id == EmpId).Select(x => new EnChatHeader {id= x.id, name = x.name, photo = x.photo, }).FirstOrDefault();
                var result = service.GetEmployeeChat(EmpId, login.id);
                return View(result);
            }
        }
    }
}
