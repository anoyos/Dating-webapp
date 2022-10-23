using james.ChatR;
using james.Helpers.General;
using james.Models;
using james.Models.DB;
using james.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Controllers
{
    public class StreamingController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        ApiModel api;
        IHubContext<ChatHub> _hubcontext;
        public StreamingController(IWebHostEnvironment hostingEnvironment, DbContextOptions<DBContext> _dbOptions, IHubContext<ChatHub> hubcontext) : base(_dbOptions, hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this.dbOption = _dbOptions;
            this._hubcontext = hubcontext;
            api = new ApiModel(_dbOptions, hubcontext);
        }
        public string CreateDefaultStream()
        {
            DBContext db = new DBContext(this.dbOption);

            if(db.streamings.Count() == 0)
            {
                var agora=api.GenerateAgoraToken();
                db.streamings.Add(new Streaming { banner = "20220822113905.jpg", title = "My First Stream", channel = agora.channel, token = agora.token, createdOn = DateTime.Now, status = (int)StreamStatus.NotStarted });
                db.SaveChanges();
            }
            return "success";

        }

        public IActionResult Streamer(int id)
        {
            DBContext db = new DBContext(this.dbOption);

            var model = new StreamerViewModel
            {
                stream = db.streamings.Where(x => x.id == id).FirstOrDefault(),
            };
           
            return View();
        }
    }
}
