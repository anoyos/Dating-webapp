using james.Helpers.Custom;
using james.Helpers.General;
using james.Models.DB;
using james.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AdminController(IWebHostEnvironment hostingEnvironment, DbContextOptions<DBContext> _dbOptions) : base(_dbOptions, hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this.dbOption = _dbOptions;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AppSetting()
        {
            DBContext db = new DBContext(this.dbOption);
            var model = db.appSettings.FirstOrDefault() ?? new AppSetting { };
            return View(model);
        }

        
        [HttpPost]
        public IActionResult AppSetting(AppSetting data)
        {
            DBContext db = new DBContext(this.dbOption);
            var e = db.appSettings.FirstOrDefault();
            if (e == null)
            {
                db.appSettings.Add(data);
                db.SaveChanges();
            }
            else
            {
                e.appVersion = data.appVersion;
                e.androidVersion = data.androidVersion;
                e.iosAppId = data.iosAppId;
                e.appSupportEmail = data.appSupportEmail;
                e.privacyPolicyUrl = data.privacyPolicyUrl;
                e.firebaseServerKey = data.firebaseServerKey;
                e.firebaseSenderId = data.firebaseSenderId;
                e.freeMaxRadius = data.freeMaxRadius;
                e.vipMaxRadius = data.vipMaxRadius;
                e.hiddenPassword = data.hiddenPassword;
                e.hiddentPasswordCnt = data.hiddentPasswordCnt;
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }
        public IActionResult Events()
        {
            DBContext db = new DBContext(this.dbOption);
            var model=db.events.Where(x => !x.isDeleted).Select(x => x).ToList();
            return View(model);
        }
        public IActionResult EventCreate(int id)
        {
            DBContext db = new DBContext(this.dbOption);
            var model = db.events.Where(x => x.id == id).FirstOrDefault() ?? new Event { };
            return View(model);
        }
        [HttpPost]
        public IActionResult EventCreate(Event data)
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                if (data.id == 0)
                {
                    data.timestamp = DateTime.Now;
                    db.events.Add(data);
                    db.SaveChanges();
                }
                else
                {
                    var e = db.events.Where(x => x.id == data.id).FirstOrDefault();
                    e.title = data.title;
                    e.location = data.location;
                    e.photo = data.photo;
                    e.startDate = data.startDate;
                    e.endDate = data.endDate;
                    e.description = data.description;                    
                    e.amount = data.amount;
                    e.isPublish = data.isPublish;
                    e.category = data.category;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Events");
        }
        public IActionResult UserApproval()
        {
            DBContext db = new DBContext(this.dbOption);
            var model = new UserApprovalViewModel
            {
                userAutoApprove = db.appSettings.Select(x => x.userAutoApprove).FirstOrDefault(),
                users = db.users.Where(x => x.roleId == (int)RoleEnum.User).Select(x => x).ToList()
            };
            return View(model);
        }

        public string UpdateUserConfig(User data)
        {
            DBContext db = new DBContext(this.dbOption);
            var u = db.users.Where(x => x.id == data.id).FirstOrDefault();
            u.isApprove = data.isApprove;
            u.isBlocked = data.isBlocked;
            u.isFlagged = data.isFlagged;
            db.SaveChanges();
            return Common.Serialize("success");
        }
        public string UpdateUserAutoApprove(bool ischeck)
        {
            DBContext db = new DBContext(this.dbOption);
            var e = db.appSettings.FirstOrDefault();
            if (e == null)
            {
                db.appSettings.Add(new Models.DB.AppSetting { userAutoApprove = ischeck });
                db.SaveChanges();
            }
            else
            {
                e.userAutoApprove = ischeck;
                db.SaveChanges();
            }
            return Common.Serialize("success");
        }
        public IActionResult Analysis()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Blogs()
        {
            DBContext db = new DBContext(this.dbOption);
            var model = db.blogs.Where(x => !x.isDeleted).ToList();
            return View(model);
        }
        public IActionResult Blog(int id)
        {
            DBContext db = new DBContext(this.dbOption);
            var model = db.blogs.Where(x => x.id == id).FirstOrDefault()?? new Blog { };
            return View(model);
        }
        [HttpPost]
        public IActionResult Blog(Blog data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if (data.id == 0)
                {
                    data.timestamp = DateTime.Now;
                    db.blogs.Add(data);
                    db.SaveChanges();
                }
                else
                {
                    var e = db.blogs.Where(x => x.id == data.id).FirstOrDefault();
                    e.title = data.title;
                    e.banner= data.banner;
                    e.description = data.description;
                    e.isPublish = data.isPublish;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Blogs");
        }
        public IActionResult BlogPreview(int id)
        {
            DBContext db = new DBContext(this.dbOption);
            var model = db.blogs.Where(x => x.id == id).FirstOrDefault();
            return View(model);
        }
        public string BlogPublish(int id , int pub)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.blogs.Where(x => x.id == id).FirstOrDefault();
                if (pub == 1)
                {
                e.isPublish = true;
                }
                else
                {
                e.isPublish = false;
                }
                db.SaveChanges();
                return Common.Serialize("success");
            }
        }
        public IActionResult Profile()
        {
            DBContext db = new DBContext(this.dbOption);
            var model=db.users.Where(x => x.id == login.id).FirstOrDefault();
            return View(model);
        }
        public string UpdatePassword(string password, string newpassword)
        {
            DBContext db = new DBContext(this.dbOption);
            var e = db.users.Where(x => x.id == login.id).FirstOrDefault();
            if (e.password != password)
            {
                return Common.Serialize(new Result { Status = ResultStatus.Error, Message = "Invalid Password" });
            }
            e.password = newpassword;
            db.SaveChanges();
            return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Password Changed" });
        }
        public IActionResult Discounts()
        {
            DBContext db = new DBContext(this.dbOption);
            var model=db.discountCoupons.Where(x=>!x.isDeleted).ToList();
            return View(model);
        }
        
        public IActionResult Discount(int id)
        {
            DBContext db = new DBContext(this.dbOption);
            var model=db.discountCoupons.Where(x => x.id == id).FirstOrDefault() ?? new DiscountCoupon { };
            return View(model);
        }
        [HttpPost]
        public IActionResult Discount(DiscountCoupon data)
        {
            DBContext db = new DBContext(this.dbOption);
            if(db.discountCoupons.Any(x=>x.id!=data.id && data.code == data.code))
            {
                ViewBag.Error = "Coupon Code Already Taken";
                return View(data);
            }
            var e = db.discountCoupons.Where(x => x.id == data.id).FirstOrDefault();
            if (e == null)
            {
                db.discountCoupons.Add(data);
                db.SaveChanges();
            }
            else
            {
                e.code = data.code;
                e.title = data.title;
                e.description = data.description;
                e.start = data.start;
                e.end = data.end;
                e.isActive = data.isActive;
                e.percentage = data.percentage;
                db.SaveChanges();
            }
            return RedirectToAction("Discounts");
        }
        public IActionResult PrizeDraw()
        {
            return View();
        }

        public ActionResult RegisterOptions(int type)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                ViewBag.screenTitle = ((DDLTypeEnum)type).ToString();
                ViewBag.type = type;
                var model = db.ddls.Where(x => !x.isDeleted && x.type == type).ToList();
                return View(model);
            }
        }
      

        public string AddUpdateDDL(DDL data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                
                if (data.id == 0)
                {
                    db.ddls.Add(data);
                   
                }
                else
                {
                    var e=db.ddls.Where(x => x.id == data.id).FirstOrDefault();
                    e.name = data.name;
                  
                }
                db.SaveChanges();
                return Common.Serialize("success");
            }
        }
        public string DeleteDDL(int id)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e=db.ddls.Where(x => x.id == id).FirstOrDefault();
                e.isDeleted = true;
                db.SaveChanges();
                return Common.Serialize("success");
            }

            }

    }
}
