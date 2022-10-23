using james.ChatR;
using james.Helpers.Custom;
using james.Helpers.Custom.Api;
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
    public class UserController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        ApiModel api;
        public UserController(IWebHostEnvironment hostingEnvironment, DbContextOptions<DBContext> _dbOptions, IHubContext<ChatHub> hubcontext) : base(_dbOptions, hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this.dbOption = _dbOptions;
            api = new ApiModel(_dbOptions, hubcontext);
        }



        public IActionResult Index(EnFilter filter)
        {
            DBContext db = new DBContext(this.dbOption);
            filter.myId = login.id;
            var users = api.GetUsers(filter);
            var model = new UserIndexViewModel
            {
                stories = api.GetStories(),
                users = users,
                ddls = db.ddls.Where(x => !x.isDeleted).ToList(),
                filter = filter,
                reportList = api.GetReportUserList(),
            };

            return View(model);
        }
        public string UpdateLike(int id)
        {
            return Common.Serialize(api.UpdateLike(login.id, id));
        }
        public string UpdateSuperLike(int id)
        {
            return Common.Serialize(api.UpdateSuperLike(login.id, id));
        }

        public string UpdateDisLike(int id)
        {
            return Common.Serialize(api.UpdateDisLike(login.id, id));
        }
        public IActionResult Profile(int id)
        {
            var model = new ProfileViewModel
            {
                profile = api.GetProfile(login.id, id, true).FirstOrDefault(),
                me = id == login.id,
            };
            return View(model);
        }

        public IActionResult QuickViewProfile(int id)
        {
            var model = new ProfileViewModel
            {
                profile = api.GetProfile(login.id, id, true).FirstOrDefault(),
                me = id == login.id,
            };
            return View(model);
        }
        public IActionResult EditProfile()
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                var model = new EditProfileViewModel
                {
                    ddls = db.ddls.Where(x => !x.isDeleted).ToList(),
                    data = api.GetEditProfile(login.id)
                };
                return View(model);
            }         
        }

        public string AddRating(UserRating data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                data.fromUserId = login.id;
                var e = db.userRatings.Where(x => x.fromUserId == data.fromUserId && x.toUserId == data.toUserId).FirstOrDefault();
                if (e != null)
                {
                    e.review = data.review;
                    e.rating = data.rating;
                }
                else
                {
                    db.userRatings.Add(new UserRating { fromUserId = data.fromUserId, toUserId = data.toUserId, rating = data.rating, review = data.review, timestamp = DateTime.Now });
                }

                db.SaveChanges();
                var u = db.users.Where(x => x.id == data.toUserId).FirstOrDefault();
                var q = db.userRatings.Where(x => x.toUserId == data.toUserId);
                u.rating = q.Count() > 0 ? q.Average(x => x.rating) : 0;
                db.SaveChanges();
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Saved Successfully" });
            }
        }
        public IActionResult GetRating(int id)
        {
            var model = api.GetRating(id);
            return View(model);
        }
        public string updateprofile(User data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                data.id = login.id;
                if (db.users.Any(x => x.id != login.id && x.source == (int)UserSourceTypeEnum.Internal && x.username == data.username))
                {
                    return Common.Serialize(new Result { Status = ResultStatus.Error, Message = "Username Already Exists" });
                }
                if (db.users.Any(x => x.id != login.id && x.source == (int)UserSourceTypeEnum.Internal && x.email == data.email))
                {
                    return Common.Serialize(new Result { Status = ResultStatus.Error, Message = "Email Already Exists" });
                }
                db.userLookingRelations.RemoveRange(db.userLookingRelations.Where(x => x.userId == data.id).ToList());
                db.userHobbies.RemoveRange(db.userHobbies.Where(x => x.userId == data.id).ToList());
                db.userPersonalities.RemoveRange(db.userPersonalities.Where(x => x.userId == data.id).ToList());
                db.userQualities.RemoveRange(db.userQualities.Where(x => x.userId == data.id).ToList());
                db.SaveChanges();



                var u = db.users.Where(x => x.id == data.id).Include(x => x.personalities).Include(x => x.hobbies).Include(x => x.lookingRelations).Include(x => x.qualities).FirstOrDefault();

                u.aboutMe = data.aboutMe;
                u.age = data.age;
                u.email = data.email;
                u.alcoholConsumptionId = data.alcoholConsumptionId;
                u.annualIncomeId = data.annualIncomeId;
                u.childrenId = data.childrenId;
                u.fetichesId = data.fetichesId;
                u.genderId = data.genderId;
                u.height = data.height;
                u.lookingGenderId = data.lookingGenderId;
                u.myProfessionId = data.myProfessionId;
                u.profession = data.profession;
                u.name = data.name;
                u.physicalTypeId = data.physicalTypeId;
                u.relationshipStatusId = data.relationshipStatusId; ;
                u.zipcode = data.zipcode;
                u.whereamiknow = data.whereamiknow;
                u.vaccineId = data.vaccineId;
                u.smokeId = data.smokeId;
                u.username = data.username;
                u.signId = data.signId;
                u.religionId = data.religionId;
                u.sexualOrientationId = data.sexualOrientationId;
                if (!string.IsNullOrEmpty(data.photo))
                    u.photo = data.photo;
                u.eduction = data.eduction;
                u.hideage = data.hideage;
                u.city = data.city;
                u.last_relationship = data.last_relationship;
                u.gelocationBydistance = data.gelocationBydistance;
                u.lookingRelations = (data.lookingRelations ?? new List<UserLookingRelation> { });
                u.personalities = (data.personalities ?? new List<UserPersonality> { });
                u.hobbies = (data.hobbies ?? new List<UserHobby> { });
                u.qualities = (data.qualities ?? new List<UserQuality> { });

                db.SaveChanges();
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "User Created Successfully" });
            }
           

        }
        public string AddAlbum(string name)
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                db.albums.Add(new Album { userId = login.id, timestamp = DateTime.Now, name = name });
                db.SaveChanges();
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Saved Successfully" });
            }
        }
        public string SaveAlbumPhoto(List<AlbumImage> list)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                foreach (var data in list)
                {
                    data.timestamp = DateTime.Now;
                    db.albumImages.Add(data);
                    db.SaveChanges();
                }
             
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Saved Successfully" });
            }
        }
        public IActionResult GetAlbums(int? id)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                id = id ?? login.id;
                var model=db.albums.Where(x => x.userId == id).Include(x=>x.images).ToList();
                return View(model);
            }

        }
        public string UpdateAlbumIsPrivate(int id,bool isPrivate)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var model = db.albums.Where(x => x.userId == login.id && x.id == id).FirstOrDefault();
                model.isPrivate = isPrivate;
                db.SaveChanges();
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Saved Successfully" });
            }
        }
        public string GenerateAlbumCode(int id)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {

                Random generator = new Random();
                String r = generator.Next(0, 1000000).ToString("D6");

                while (db.hiddenAlbums.Any(x=>x.code==r && x.album.userId==login.id))
                {
                    r = generator.Next(0, 1000000).ToString("D6");
                }
                db.hiddenAlbums.Add(new HiddenAlbum {  timestamp=DateTime.Now, albumId=id, code=r, status=(int)HiddenAlbumCodeStatus.NotUsed});             
                db.SaveChanges();
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Saved Successfully", Data = r });
            }
        }
        public string ApplyAlbumCode(int userId,string code)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e=db.hiddenAlbums.Where(x => x.album.userId == userId && x.code == code && x.status == (int)HiddenAlbumCodeStatus.NotUsed).FirstOrDefault();
                if (e != null)
                {
                    e.status = (int)HiddenAlbumCodeStatus.Used;
                    e.userId = login.id;
                    db.SaveChanges();
                    return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Saved Successfully" });
                }
                else
                {
                    db.SaveChanges();
                    return Common.Serialize(new Result { Status = ResultStatus.Error, Message = "Invalid Code", });
                }
               
            }
        }
        public IActionResult Likes()
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                var coord = db.users.Where(x => x.id == login.id).Select(x => new { x.lat, x.lng }).FirstOrDefault();

                var likes = db.matches.Where(x => x.fromUserId == login.id && (x.type == (int)MatchTypeEnum.SuperLike || x.type == (int)MatchTypeEnum.Like)).Include(x => x.toUser).ThenInclude(x => x.vaccine)
               .Select(x => new EnUser
               {
                   id = x.toUser.id,
                   photo = x.toUser.photo,
                   name = x.toUser.name,
                   age = x.toUser.hideage ? x.fromUser.age : default(int?),
                   likes = x.toUser.likes,
                   rating = x.toUser.rating,
                   eduction = x.toUser.eduction,
                   profession = x.toUser.profession,
                   vaccine = x.toUser.vaccineId.HasValue ? x.toUser.vaccine.name : "",
                   distance = x.toUser.gelocationBydistance == 1 ? Common.GetDistance(x.toUser.lat, x.toUser.lng, coord.lat, coord.lng) : default(double?)
               }).OrderByDescending(x => x.id).ToList();
                var model = new LikeViewModel { list=likes };
                return View(model);
            }         
        }
        public IActionResult DisLike()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var coord = db.users.Where(x => x.id == login.id).Select(x => new { x.lat, x.lng }).FirstOrDefault();
                var likes = db.matches.Where(x => x.fromUserId == login.id && x.type == (int)MatchTypeEnum.Dislike).Include(x => x.toUser).ThenInclude(x => x.vaccine)
               .Select(x => new EnUser
               {
                   id = x.toUser.id,
                   photo = x.toUser.photo,
                   name = x.toUser.name,
                   age = x.toUser.hideage ? x.fromUser.age : default(int?),
                   likes = x.toUser.likes,
                   rating = x.toUser.rating,
                   eduction = x.toUser.eduction,
                   profession = x.toUser.profession,
                   vaccine = x.toUser.vaccineId.HasValue ? x.toUser.vaccine.name : "",
                   distance = x.toUser.gelocationBydistance == 1 ? Common.GetDistance(x.toUser.lat, x.toUser.lng, coord.lat, coord.lng) : default(double?)
               }).OrderByDescending(x => x.id).ToList();
                var model = new LikeViewModel { list = likes };
                return View(model);
            }
        }
        public IActionResult Matches()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var skipUids = db.matches.Where(x => x.fromUserId == login.id && (x.type == (int)MatchTypeEnum.SuperLike || x.type == (int)MatchTypeEnum.Like || x.type == (int)MatchTypeEnum.Dislike)).Select(x => x.toUserId).ToList();
                var coord = db.users.Where(x => x.id == login.id).Select(x => new { x.lat, x.lng }).FirstOrDefault();
                var matches = db.matches.Where(x => (x.fromUserId == login.id || x.toUserId == login.id) && (x.type == (int)MatchTypeEnum.Match)).Include(x => x.toUser).ThenInclude(x => x.vaccine)
                    .Select(x => new EnUser
                    {
                        id = x.fromUserId == login.id ? x.toUser.id : x.fromUserId,
                        photo = x.fromUserId == login.id ? x.toUser.photo : x.fromUser.photo,
                        name = x.fromUserId == login.id ? x.toUser.name : x.fromUser.name,
                        age = x.fromUserId == login.id ? (!x.toUser.hideage ? x.toUser.age : default(int?)) : (!x.fromUser.hideage ? x.fromUser.age : default(int?)),
                        likes = x.fromUserId == login.id ? x.toUser.likes : x.fromUser.likes,
                        rating = x.fromUserId == login.id ? x.toUser.rating : x.fromUser.rating,
                        eduction = x.fromUserId == login.id ? x.toUser.eduction : x.fromUser.eduction,
                        profession = x.fromUserId == login.id ? x.toUser.profession : x.fromUser.profession,
                        vaccine = x.fromUserId == login.id ? (x.toUser.vaccineId.HasValue ? x.toUser.vaccine.name : "") : (x.fromUser.vaccineId.HasValue ? x.fromUser.vaccine.name : ""),
                        distance = x.toUser.gelocationBydistance == 1 ? Common.GetDistance(x.toUser.lat, x.toUser.lng, coord.lat, coord.lng) : default(double?)
                    }).OrderByDescending(x => x.id).ToList();
                var model = new LikeViewModel { list = matches };
                return View(model);
            }
        }
   
        public IActionResult Events()
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                var cdate = DateTime.Now.Date;
                var model = new UserEventListModel
                {
                    events = db.events.Where(x => x.isPublish && cdate>= x.startDate  && cdate<= x.endDate ).ToList()
                };
                return View(model);
            }
  
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        public IActionResult TermsConditions()
        {
            return View();
        }
        public IActionResult ProfileStatics()
        {
            var model=api.GetProfileStats(login.id);
            return View(model);
        }
        public IActionResult Diary()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                DiaryViewModel model = new DiaryViewModel
                {
                    list = db.diaries.Where(x => x.userId == login.id).ToList()
                };
                return View(model);
            }                
        }

        public string SaveDiary(Diary data)
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                db.diaries.Add(new Diary { dateTime = data.dateTime, title = data.title, description = data.description, photo = data.photo, userId = login.id });
                db.SaveChanges();
                return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Diary Added", });
            }
        }
        public string AddReportUser(EnReport data)
        {
            data.fromUserId = login.id;
            api.AddReportUser(data);
            return Common.Serialize(new Result { Status = ResultStatus.Success, Message = "Saved Successfully" });
        }

        public IActionResult Event(int id)
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                var model = new UserEventModel
                {
                    data = db.events.Where(x => x.id == id).FirstOrDefault(),
                };
                return View(model);
            }            
        }
        public IActionResult Messages()
        {
            return View();
        }
        public IActionResult Streaming()
        {
            return View();
        }
        public IActionResult EmojiandGifts()
        {
            return View();
        }
        public IActionResult PrizeDraw()
        {
            return View();
        }
        public IActionResult VIPAccount()
        {
            return View();
        }
        public IActionResult StreamingDetail()
        {
            return View();
        }
    }
}
