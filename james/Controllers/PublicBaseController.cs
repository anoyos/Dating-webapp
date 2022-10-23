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
    public class PublicBaseController : Controller
    {
        protected DbContextOptions<DBContext> dbOption;
        IHostingEnvironment hostingEnvironment;
        public PublicBaseController(DbContextOptions<DBContext> dbOptions, IHostingEnvironment hostingEnvironment)
        {
            this.dbOption = dbOptions;
            this.hostingEnvironment = hostingEnvironment;
        }
        protected tblLogin login;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            using (DBContext db = new DBContext(dbOption))
            {
                base.OnActionExecuting(context);
                SessionHelper sessionHelper = new SessionHelper(db, this.Request, this.Response);
                login = sessionHelper.get();
                ViewBag.login = login;
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
