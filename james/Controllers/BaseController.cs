using james.Helpers.Custom;
using james.Helpers.General;
using james.Models.DB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Controllers
{
    public class BaseController : Controller
    {
        protected DbContextOptions<DBContext> dbOption;
        IWebHostEnvironment hostingEnvironment;
        public BaseController(DbContextOptions<DBContext> dbOptions, IWebHostEnvironment hostingEnvironment)
        {

            this.dbOption = dbOptions;
            this.hostingEnvironment = hostingEnvironment;
        }
        protected tblLogin login;
        List<string> bypassactions = new List<string> { "invoice" };
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            DBContext db = new DBContext(dbOption);
            SessionHelper sessionHelper = new SessionHelper(db, this.Request, this.Response);
            login = sessionHelper.get();

            var action = context.RouteData.Values["action"].ToString().ToLower();
            var controller = context.RouteData.Values["controller"].ToString().ToLower();
            if (bypassactions.Contains(action))
            {
                return;
            }
            if (login == null)
            {
                context.Result = new RedirectResult("/home/logout");
            }
            else
            {
                ViewBag.login = login;
                ViewBag.userId = login.id;
                ViewBag.id = login.id;
                ViewBag.name = login.name;
                ViewBag.basePath = this.hostingEnvironment.WebRootPath;
                ViewBag.roleId = login.roleId;
                ViewBag.photo = login.photo;          
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }



    }
}
