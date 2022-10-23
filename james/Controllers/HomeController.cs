using james.Helpers.Custom;
using james.Helpers.General;
using james.Models.DB;
using james.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace james.Controllers
{
    public class HomeController : PublicBaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IHostingEnvironment hostingEnvironment, DbContextOptions<DBContext> _dbOptions) : base(_dbOptions, hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this.dbOption = _dbOptions;
        }
        public string delfb()
        {
            return "succcess";
        }


        [HttpPost]
        public async Task<string> FacebookGoogleLogin(LoginViewModel model, string returnUrl)
        {
            DBContext db = new DBContext(this.dbOption);

            var user = db.users.Where(x => !x.isdeleted && x.source == model.source && x.username == model.id).FirstOrDefault();
            if (user == null)
            {
                db.users.Add(new Models.DB.User { name = model.name, email = model.email, source = model.source, roleId = (int)RoleEnum.User, username = model.id, password = Constants.socialPassword, });
                db.SaveChanges();
                user = db.users.Where(x => !x.isdeleted && x.source == model.source && x.username == model.id).FirstOrDefault();
            }
            SessionHelper session = new SessionHelper(db, this.Request, Response);
            session.set(model.id);
            return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Login Successfully" });
        }


        public async Task<IActionResult> GoogleLogin([FromQuery] string access_token)
        {

            //club@james.pixtechcreation.com  A9b8c7d6e!
            //client-id: 449769173150-e9b3veat78lrhsued5h85j26ub93ildu.apps.googleusercontent.com
            //client-secret: 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
            var f = Request.GetEncodedUrl();
            using (var client = new HttpClient())
            {
                if (access_token == null)
                {
                    return View();
                }

                try
                {
                    var webUrl = "https://www.googleapis.com/oauth2/";
                    var uri = "v3/userinfo";
                    client.BaseAddress = new Uri(webUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.ConnectionClose = true;


                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                    var result = await client.PostAsync(uri, null);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var rd = await result.Content.ReadAsStringAsync();
                        var login = Common.Deserialize<GoogleLoginResp>(rd);
                        var internalResp = await FacebookGoogleLogin(new LoginViewModel
                        {
                            id = login.sub,
                            source = (int)UserSourceTypeEnum.Google,
                            email = login.sub + "@google.com",
                            name = login.name,
                            password = Constants.socialPassword,
                        }, null);
                        var resp = Common.Deserialize<Result>(internalResp);
                        if (resp.Status==ResultStatus.Success)
                        {
                            return RedirectToAction("Index", "User");
                        }                        

                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {

                }

                ViewBag.message = "Unable To Login";
                return View();
            }


        }
        public IActionResult AdminLogin()
        {
            return View(new LoginViewModel { });
        }
        public string CreateAdmin()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if(!db.users.Any(x => x.username == "admin"))
                {
                    db.users.Add(new Models.DB.User
                    {
                        username = "admin",
                        password = "123",
                        roleId = (int)RoleEnum.Admin,
                        source = (int)UserSourceTypeEnum.Internal
                    });
                    db.SaveChanges();
                }
                return "success";
            }
        }
        [HttpPost]
        public IActionResult AdminLogin(LoginViewModel model)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var user = db.users.Where(x => !x.isdeleted && x.roleId == (int)RoleEnum.Admin && x.source == (int)UserSourceTypeEnum.Internal && x.username == model.username && x.password == model.password).FirstOrDefault();
                if (user != null)
                {
                    SessionHelper session = new SessionHelper(db, this.Request, Response);
                    session.set(model.username);
                    return RedirectToAction("dashboard", "admin");
                }
                else
                {
                    model.error = true;
                    model.message = "Invalid Username & Password";
                    return View(model);
                }
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Terms()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View(new ForgotViewModel { });
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotViewModel model)
        {
            DBContext db = new DBContext(this.dbOption);
            var e = db.users.Where(x => x.email == model.email && x.source == (int)UserSourceTypeEnum.Internal).FirstOrDefault();

            if (e != null)
            {
                string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                var code = "7777";
                var template = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, "email_templates", "forgot_password.html"));
                template = template.Replace("@{{name}}", e.name);
                template = template.Replace("@{{code}}", code);
                var uid = Guid.NewGuid().ToString();
                db.verificationCodes.Add(new VerificationCode { uid = uid, code = code, timestamp = DateTime.Now, userId = e.id, });
                db.SaveChanges();
                model.error = false;
                model.message = "Reset Code Sent On Email";
                return RedirectToAction("ForgotCode", "Home", new { uid = uid });
            }
            else
            {
                model.error = true;
                model.message = "Invalid Email";
                return View(model);
            }
        }
    
  
    public IActionResult ForgotCode(string uid)
    {
            DBContext db = new DBContext(this.dbOption);
            var model = db.verificationCodes.Where(x => x.uid == uid).Include(x => x.user).FirstOrDefault();
            if (model == null)
            {
                return View(new ForgotCodeViewModel { error = true, message = "Invalid Link" });
            }
            return View(new ForgotCodeViewModel { email = model.user.email, uid = uid });
        }
        [HttpPost]
        public IActionResult ForgotCode(ForgotCodeViewModel model)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var c = db.verificationCodes.Where(x => x.uid == model.uid && x.code == model.code).Include(x => x.user).FirstOrDefault();
                if (c != null)
                {
                    c.user.password = model.password;
                    db.verificationCodes.Remove(c);
                    db.SaveChanges();
                    return View(new ForgotCodeViewModel { error = false, message = "Password Reset Successfully", email = c.user.email });
                }
                return View(new ForgotCodeViewModel { error = true, message = "Invalid Code", });
            }
        }
        public IActionResult Login()
        {
            return View(new LoginViewModel { });
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var user=db.users.Where(x =>!x.isdeleted && x.roleId==(int)RoleEnum.User && x.source==(int)UserSourceTypeEnum.Internal && x.username == model.username && x.password == model.password).FirstOrDefault();
                if (user != null)
                {
                    SessionHelper session = new SessionHelper(db, this.Request, Response);
                    session.set(model.username);
                    return RedirectToAction("index", "user");
                }
                else
                {
                    model.error = true;
                    model.message = "Invalid Username & Password";
                    return View(model);
                }
            }
        }
        public IActionResult Register()
        {
            DBContext db = new DBContext(this.dbOption);
            var list=db.ddls.Where(x => !x.isDeleted).ToList();
            var model = new RegisterViewModel { ddls = list };
            return View(model);
        }
        public string SaveRegister(User data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if (db.users.Any(x => x.source == (int)UserSourceTypeEnum.Internal && x.username == data.username))
                {
                    return Common.Serialize(new Result { Status = ResultStatus.Error, Message = "Username Already Exists" });
                }
                if (db.users.Any(x => x.source == (int)UserSourceTypeEnum.Internal && x.email == data.email))
                {
                    return Common.Serialize(new Result { Status = ResultStatus.Error, Message = "Email Already Exists" });
                }
                data.roleId = (int)RoleEnum.User;
                data.source = (int)UserSourceTypeEnum.Internal;
                db.users.Add(data);
                db.SaveChanges();
                SessionHelper session = new SessionHelper(db, this.Request, Response);
                session.set(data.username);
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "User Created Successfully" });
            }
        }
        public string CheckUserAvailability(string username,string email)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if (db.users.Any(x => x.source == (int)UserSourceTypeEnum.Internal && x.username == username))
                {
                    return Common.Serialize(new Result { Status = ResultStatus.Error, Message = "Username Already Exists" });
                }
                if (db.users.Any(x => x.source == (int)UserSourceTypeEnum.Internal && x.email == email))
                {
                    return Common.Serialize(new Result { Status = ResultStatus.Error, Message = "Email Already Exists" });
                }
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "User available" });
            }
        }

        public async Task Logout()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                SessionHelper sessionHelper = new SessionHelper(db, this.Request, this.Response);
                sessionHelper.delete();
                Response.Redirect("/Home/Index");
            }
        }

        public IActionResult Index()
        {
            DBContext db = new DBContext(this.dbOption);
            SessionHelper session = new SessionHelper(db, Request, Response);
          ViewBag.isCookieAccept= session.getCookieAccept();
            return View();
        }


        public string AcceptCookies(bool accept)
        {
            DBContext db = new DBContext(this.dbOption);
            SessionHelper session = new SessionHelper(db, Request, Response);
            session.setCookieAccept(accept);
                return Common.Serialize("success");
        }
        public IActionResult Temp()
        {
            return View();
        }

    }
}
