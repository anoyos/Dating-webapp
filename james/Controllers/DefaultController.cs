using james.ChatR;
using james.Helpers.Custom;
using james.Helpers.Custom.Api;
using james.Helpers.General;
using james.Models;
using james.Models.DB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace james.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        DbContextOptions<DBContext> dbOption;
        private IHostingEnvironment _env;
        ApiModel api;
        IConfiguration _configuration;
        IHubContext<ChatHub> _hubcontext;
        public DefaultController(IConfiguration configuration, DbContextOptions<DBContext> _dbOptions, IHostingEnvironment environment, IHubContext<ChatHub> hubcontext)
        {
            this.dbOption = _dbOptions;
            _env = environment;
            api = new ApiModel(_dbOptions, hubcontext);
            this._configuration = configuration;
            this._hubcontext = hubcontext;
        }

        [HttpGet]
        [Route("CheckUserAvailability")]
        public async Task<Result> CheckUserAvailability(string username, string email)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if (db.users.Any(x => x.source == (int)UserSourceTypeEnum.Internal && x.username == username))
                {
                    return new Result { Status = ResultStatus.Error, Message = "Username Already Exists" };
                }
                if (db.users.Any(x => x.source == (int)UserSourceTypeEnum.Internal && x.email == email))
                {
                    return new Result { Status = ResultStatus.Error, Message = "Email Already Exists" };
                }
                return new Result { Status = ResultStatus.Success, Message = "User available" };
            }
        }

        [HttpGet]
        [Route("GetReportUserList")]
        public async Task<Result<EnReportUserOptionList>> GetReportUserList()
        {
            var reasons=api.GetReportUserList();
            return new Result<EnReportUserOptionList> { Status = ResultStatus.Success, Message = "Success", Data = reasons };
        }
  
        [HttpPost]
        [Route("Login")]
        public async Task<Result<EnLogin>> Login(string username, string password)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var user = db.users.Where(x => !x.isdeleted && x.roleId == (int)RoleEnum.User && x.source == (int)UserSourceTypeEnum.Internal && x.username == username && x.password == password).FirstOrDefault();
                if (user != null)
                {
                    var login = new EnLogin
                    {
                        id = user.id,
                        name = user.name,
                        username = user.username,
                        email = user.email,
                        photo = user.photo,
                        source = user.source,
                    };
                    return new Result<EnLogin> { Status = ResultStatus.Success, Message = "Login Successfully", Data = new List<EnLogin> { login } };
                }
            }
            return new Result<EnLogin> { Status = ResultStatus.Error, Message = "Invalid Username or Password" };
        }

        [HttpPost]
        [Route("SocialLogin")]
        public async Task<Result<EnLogin>> SocialLogin(EnSocialLogin data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var user = db.users.Where(x => !x.isdeleted && x.source == data.source && x.username == data.id).FirstOrDefault();
                if (user == null)
                {
                    db.users.Add(new Models.DB.User { name = data.name, email = data.email, source = data.source, roleId = (int)RoleEnum.User,username=data.id, password = Constants.socialPassword, });
                    db.SaveChanges();
                    user = db.users.Where(x => !x.isdeleted && x.source == data.source && x.username == data.id).FirstOrDefault();
                }
                var login = new EnLogin
                {
                    id = user.id,
                    name = user.name,
                    username = user.username,
                    email = user.email,
                    photo = user.photo,
                    source = user.source,
                };
                return new Result<EnLogin> { Status = ResultStatus.Success, Message = "Login Successfully", Data = new List<EnLogin> { login } };
            }

        }
        [HttpPost]
        [Route("GetEditProfile")]
        public async Task<Result<EnRegisterEdit>> GetEditProfile(int id)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                try
                {
                    var result = api.GetEditProfile(id);
                    return new Result<EnRegisterEdit> { Status = ResultStatus.Success, Message = "User Retrieve Successfully", Data = new List<EnRegisterEdit> { result } };
                }
                catch (Exception ex)
                {
                    return new Result<EnRegisterEdit> { Status = ResultStatus.Error, Message = ex.ToString() + "\n" + (ex.InnerException != null ? ex.InnerException.ToString() : "") };
                }
            }
        }
        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<Result> UpdateProfile(EnRegister data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if (db.users.Any(x => x.id != data.id && x.source == (int)UserSourceTypeEnum.Internal && x.username == data.username))
                {
                    return new Result { Status = ResultStatus.Error, Message = "Username Already Exists" };
                }
                if (db.users.Any(x => x.id != data.id && x.source == (int)UserSourceTypeEnum.Internal && x.email == data.email))
                {
                    return new Result { Status = ResultStatus.Error, Message = "Email Already Exists" };
                }
                if (!string.IsNullOrEmpty(data.photo))
                {
                    data.photo = Common.Base64ToFile(_env.WebRootPath, data.photo, ".jpg");
                }
                try
                {
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
                    u.password = data.password;
                    u.zipcode = data.zipcode;
                    u.whereamiknow = data.whereamiknow;
                    u.vaccineId = data.vaccineId;
                    u.smokeId = data.smokeId;
                    u.username = data.username;
                    u.signId = data.signId;
                    u.city = data.city;
                    u.religionId = data.religionId;
                    u.sexualOrientationId = data.sexualOrientationId;
                    if (!string.IsNullOrEmpty(data.photo))
                        u.photo = data.photo;
                    u.eduction = data.eduction;
                    u.hideage = data.hideage;
                    u.city = data.city;
                    u.last_relationship = data.last_relationship;
                    u.gelocationBydistance = data.gelocationBydistance;
                    u.lookingRelations = (data.lookingRelations ?? new List<int> { }).Select(x => new UserLookingRelation { relationId = x }).ToList();
                    u.personalities = (data.personalities ?? new List<int> { }).Select(x => new UserPersonality { personalityId = x }).ToList();
                    u.hobbies = (data.hobbies ?? new List<int> { }).Select(x => new UserHobby { hobbyId = x }).ToList();
                    u.qualities = (data.qualities ?? new List<int> { }).Select(x => new UserQuality { qualityId = x }).ToList();

                    db.SaveChanges();
                    return new Result { Status = ResultStatus.Success, Message = "User Updated Successfully" };
                }
                catch (Exception ex)
                {
                    return new Result { Status = ResultStatus.Error, Message = ex.ToString() + "\n" + (ex.InnerException != null ? ex.InnerException.ToString() : "") };
                }
            }
        }
        [HttpPost]
        [Route("GetUsers")]
        public async Task<Result<EnUser>> GetUsers(EnFilter filter)
        {
            try
            {
                var users= api.GetUsers(filter);
                return new Result<EnUser> { Status = ResultStatus.Success, Message = "User Retrieve Successfully", Data = users };

            }
            catch (Exception ex)
            {
                return new Result<EnUser> { Status = ResultStatus.Error, Message = "Failed To Retrieve Error:" + ex.ToString() };
            }

        }

        [HttpPost]
        [Route("UpdateLatLng")]
        public async Task<Result> UpdateLatLng(int userId, string lat, string lng)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var u = db.users.Where(x => x.id == userId).FirstOrDefault();
                u.lat = lat;
                u.lng = lng;
                u.lastLocTimestamp = DateTime.Now;
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = "Location Updated", };
            }
        }

        [HttpPost]
        [Route("UpdateLike")]
        public async Task<Result> UpdateLike(int fromUserId, int toUserId)
        {
            return await api.UpdateLike(fromUserId, toUserId);        
        }
        [HttpPost]
        [Route("UpdateSuperLike")]
        public async Task<Result> UpdateSuperLike(int fromUserId, int toUserId)
        {
            return await api.UpdateSuperLike(fromUserId, toUserId);
        }
        [HttpPost]
        [Route("UpdateProfileView")]
        public async Task<Result> UpdateProfileView(int userId)
        {
            return api.UpdateProfileView(userId);
        }
        [HttpGet]
        [Route("GetProfileStats")]
        public async Task<Result<EnProfileStats>> GetProfileStats(int userId)
        {
            var data = api.GetProfileStats(userId);
            return new Result<EnProfileStats> { Status = ResultStatus.Success, Message = "Stats Retrieve Successfully", Data = new List<EnProfileStats> { data } };
        }
        
        [HttpPost]
        [Route("UpdateDisLike")]
        public async Task<Result> UpdateDisLike(int fromUserId, int toUserId)
        {
            return api.UpdateDisLike(fromUserId, toUserId);           
        }
        [HttpGet]
        [Route("GetMatches")]
        public async Task<Result<EnMatch>> GetMatches(int userId)
        {
            try
            {
                using (DBContext db = new DBContext(this.dbOption))
                {
                    var coord = db.users.Where(x => x.id == userId).Select(x => new { x.lat, x.lng }).FirstOrDefault();
                    var likesMe = db.matches.Where(x => x.toUserId == userId && (x.type == (int)MatchTypeEnum.SuperLike || x.type == (int)MatchTypeEnum.Like)).Include(x => x.fromUser).ThenInclude(x => x.vaccine)
                        .Select(x => new EnUser
                        {
                            id = x.fromUser.id,
                            photo = x.fromUser.photo,
                            name = x.fromUser.name,
                            age = x.fromUser.hideage ? x.fromUser.age : default(int?),
                            likes = x.fromUser.likes,
                            rating = x.fromUser.rating,
                            eduction = x.fromUser.eduction,
                            profession = x.fromUser.profession,
                            vaccine = x.fromUser.vaccineId.HasValue ? x.fromUser.vaccine.name : "",
                            distance = x.fromUser.gelocationBydistance == 1 ? Common.GetDistance(x.fromUser.lat, x.fromUser.lng, coord.lat, coord.lng) : default(double?)
                        }).OrderByDescending(x => x.id).ToList();
                    var likes = db.matches.Where(x => x.fromUserId == userId && (x.type == (int)MatchTypeEnum.SuperLike || x.type == (int)MatchTypeEnum.Like)).Include(x => x.toUser).ThenInclude(x => x.vaccine)
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

                    var matches = db.matches.Where(x => (x.fromUserId == userId || x.toUserId == userId) && (x.type == (int)MatchTypeEnum.Match)).Include(x => x.toUser).Include(x => x.fromUser).ThenInclude(x => x.vaccine)
                    .Select(x => new EnUser
                    {
                        id = x.fromUserId == userId ? x.toUser.id : x.fromUserId,
                        photo = x.fromUserId == userId ? x.toUser.photo : x.fromUser.photo,
                        name = x.fromUserId == userId ? x.toUser.name : x.fromUser.name,
                        age = x.fromUserId == userId ? (!x.toUser.hideage ? x.toUser.age : default(int?)) : (!x.fromUser.hideage ? x.fromUser.age : default(int?)),
                        likes = x.fromUserId == userId ? x.toUser.likes : x.fromUser.likes,
                        rating = x.fromUserId == userId ? x.toUser.rating : x.fromUser.rating,
                        eduction = x.fromUserId == userId ? x.toUser.eduction : x.fromUser.eduction,
                        profession = x.fromUserId == userId ? x.toUser.profession : x.fromUser.profession,
                        vaccine = x.fromUserId == userId ? (x.toUser.vaccineId.HasValue ? x.toUser.vaccine.name : "") : (x.fromUser.vaccineId.HasValue ? x.fromUser.vaccine.name : ""),
                        distance = x.toUser.gelocationBydistance == 1 ? Common.GetDistance(x.toUser.lat, x.toUser.lng, coord.lat, coord.lng) : default(double?)
                    }).OrderByDescending(x => x.id).ToList();
                    var result = new EnMatch { matches = matches, likes = likes, likesMe = likesMe };
                    return new Result<EnMatch> { Status = ResultStatus.Success, Message = "User Retrieve Successfully", Data = new List<EnMatch> { result } };
                }
            }
            catch (Exception ex)
            {
                return new Result<EnMatch> { Status = ResultStatus.Error, Message = "Failed To Retrieve Error:" + ex.ToString() };
            }
        }




        //Gender,looking_relation,looking_gender,annual_income, alchol,smokes,sexual_orientation,relationship_status,vacine,children,personlity,qualities,my prffession, physicalType,Religon,hobbies,sign
        [HttpGet]
        [Route("GetDDLS")]
        public async Task<Result<EnRegisterDDL>> GetDDLS()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var data = new EnRegisterDDL
                {
                    Alcohol = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Alcohol).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    AnnualIncome = db.ddls.Where(x => x.type == (int)DDLTypeEnum.AnnualIncome).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Children = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Children).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Gender = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Gender).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Hobbies = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Hobbies).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    LookingGender = db.ddls.Where(x => x.type == (int)DDLTypeEnum.LookingGender).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    LookingRelation = db.ddls.Where(x => x.type == (int)DDLTypeEnum.LookingRelation).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Myproffession = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Myproffession).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Personlity = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Personlity).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    PhysicalType = db.ddls.Where(x => x.type == (int)DDLTypeEnum.PhysicalType).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Qualities = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Qualities).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    RelationshipStatus = db.ddls.Where(x => x.type == (int)DDLTypeEnum.RelationshipStatus).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Religon = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Religon).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    SexualOrientation = db.ddls.Where(x => x.type == (int)DDLTypeEnum.SexualOrientation).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Sign = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Sign).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Smokes = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Smokes).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Vacine = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Vacine).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                    Fetches = db.ddls.Where(x => x.type == (int)DDLTypeEnum.Fetches).Select(x => new EnDDL { id = x.id, name = x.name, type = x.type }).ToList(),
                };
                return new Result<EnRegisterDDL> { Status = ResultStatus.Success, Message = ResultStatus.Success.ToString(), Data = new List<EnRegisterDDL> { data } };
            }
        }
        [HttpPost]
        [Route("AddDDL")]
        public async Task<Result> AddDDL(List<EnDDL> list)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                db.ddls.AddRange(list.Select(x => new DDL { name = x.name, type = x.type, }).ToList());
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = ResultStatus.Success.ToString() };
            }
        }


        [HttpPost]
        [Route("Signup")]
        public async Task<Result> Signup(EnRegister data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if (db.users.Any(x => x.source == (int)UserSourceTypeEnum.Internal && x.username == data.username))
                {
                    return new Result { Status = ResultStatus.Error, Message = "Username Already Exists" };
                }
                if (db.users.Any(x => x.source == (int)UserSourceTypeEnum.Internal && x.email == data.email))
                {
                    return new Result { Status = ResultStatus.Error, Message = "Email Already Exists" };
                }
                if (!string.IsNullOrEmpty(data.photo))
                {
                    data.photo = Common.Base64ToFile(_env.WebRootPath, data.photo, ".jpg");
                }
                try
                {
                    var u = new Models.DB.User
                    {
                        aboutMe = data.aboutMe,
                        age = data.age,
                        email = data.email,
                        alcoholConsumptionId = data.alcoholConsumptionId,
                        annualIncomeId = data.annualIncomeId,
                        childrenId = data.childrenId,
                        fetichesId = data.fetichesId,
                        genderId = data.genderId,
                        height = data.height,
                        lookingGenderId = data.lookingGenderId,
                        myProfessionId = data.myProfessionId,
                        name = data.name,
                        physicalTypeId = data.physicalTypeId,
                        relationshipStatusId = data.relationshipStatusId,
                        password = data.password,
                        zipcode = data.zipcode,
                        whereamiknow = data.whereamiknow,
                        vaccineId = data.vaccineId,
                        smokeId = data.smokeId,
                        username = data.username,
                        signId = data.signId,
                        religionId = data.religionId,
                        sexualOrientationId = data.sexualOrientationId,
                        photo = data.photo,
                        eduction = data.eduction,
                        city = data.city,
                        hideage = data.hideage,
                        last_relationship = data.last_relationship,
                        gelocationBydistance = data.gelocationBydistance,
                        lookingRelations = (data.lookingRelations ?? new List<int> { }).Select(x => new UserLookingRelation { relationId = x }).ToList(),
                        personalities = (data.personalities ?? new List<int> { }).Select(x => new UserPersonality { personalityId = x }).ToList(),
                        hobbies = (data.hobbies ?? new List<int> { }).Select(x => new UserHobby { hobbyId = x }).ToList(),
                        qualities = (data.qualities ?? new List<int> { }).Select(x => new UserQuality { qualityId = x }).ToList(),
                        roleId = (int)RoleEnum.User,
                        source = (int)UserSourceTypeEnum.Internal
                    };
                    db.users.Add(u);
                    db.SaveChanges();
                    return new Result { Status = ResultStatus.Success, Message = "Username Registered Successfully" };
                }
                catch (Exception ex)
                {
                    return new Result { Status = ResultStatus.Error, Message = ex.ToString() + "\n" + (ex.InnerException != null ? ex.InnerException.ToString() : "") };
                }
            }
        }
        [HttpPost]
        [Route("SyncDefaultData")]
        public async Task<Result> SyncDefaultData()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                db.ddls.RemoveRange(db.ddls.ToList());
                db.SaveChanges();
                var list = new List<DDL>
                {
                    new DDL{ name="Male",type=(int)DDLTypeEnum.Gender},
                    new DDL{ name="Female",type=(int)DDLTypeEnum.Gender},
                    new DDL{ name="Other",type=(int)DDLTypeEnum.Gender},


                    new DDL{ name="Friendship",type=(int)DDLTypeEnum.LookingRelation},
                    new DDL{ name="Flirting",type=(int)DDLTypeEnum.LookingRelation},
                    new DDL{ name="Casual Sex",type=(int)DDLTypeEnum.LookingRelation},
                    new DDL{ name="Dating",type=(int)DDLTypeEnum.LookingRelation},
                    new DDL{ name="Marriage",type=(int)DDLTypeEnum.LookingRelation},
                    new DDL{ name="Love at the best age",type=(int)DDLTypeEnum.LookingRelation},
                    new DDL{ name="Travel Company",type=(int)DDLTypeEnum.LookingRelation},

                    new DDL{ name="Male",type=(int)DDLTypeEnum.LookingGender},
                    new DDL{ name="Female",type=(int)DDLTypeEnum.LookingGender},
                    new DDL{ name="Other",type=(int)DDLTypeEnum.LookingGender},

                    new DDL{ name="10k",type=(int)DDLTypeEnum.AnnualIncome},
                    new DDL{ name="20k",type=(int)DDLTypeEnum.AnnualIncome},

                    new DDL{ name="I rather not to say",type=(int)DDLTypeEnum.Alcohol},
                    new DDL{ name="Never drink",type=(int)DDLTypeEnum.Alcohol},
                    new DDL{ name="Socially ",type=(int)DDLTypeEnum.Alcohol},
                    new DDL{ name="Regularly ",type=(int)DDLTypeEnum.Alcohol},

                    new DDL{ name="Yes",type=(int)DDLTypeEnum.Smokes},
                    new DDL{ name="No",type=(int)DDLTypeEnum.Smokes},

                    new DDL{ name="Heterosexual",type=(int)DDLTypeEnum.SexualOrientation},
                    new DDL{ name="Homosexual",type=(int)DDLTypeEnum.SexualOrientation},
                    new DDL{ name="Bisexual",type=(int)DDLTypeEnum.SexualOrientation},
                    new DDL{ name="Asexual",type=(int)DDLTypeEnum.SexualOrientation},

                    new DDL{ name="Single",type=(int)DDLTypeEnum.RelationshipStatus},
                    new DDL{ name="Engaged",type=(int)DDLTypeEnum.RelationshipStatus},
                    new DDL{ name="Married",type=(int)DDLTypeEnum.RelationshipStatus},
                    new DDL{ name="Divorced",type=(int)DDLTypeEnum.RelationshipStatus},

                    new DDL{ name="Fully vaccinated",type=(int)DDLTypeEnum.Vacine},
                    new DDL{ name="Partially vaccinated",type=(int)DDLTypeEnum.Vacine},
                    new DDL{ name="None",type=(int)DDLTypeEnum.Vacine},

                    new DDL{ name="Yes",type=(int)DDLTypeEnum.Children},
                    new DDL{ name="No",type=(int)DDLTypeEnum.Children},


                    new DDL{ name="Soft Hearted",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Counselor",type=(int)DDLTypeEnum.Personlity},

                    new DDL{ name="Mastermind",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Giver",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Craftsman",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Provider",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Idealist",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Performer",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Champion",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Doer",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Supervisor",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Commander",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Thinker",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Nurturer",type=(int)DDLTypeEnum.Personlity},
                    new DDL{ name="Visonary",type=(int)DDLTypeEnum.Personlity},


                    new DDL{ name="Openness to change",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Humility",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Loyalty",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Accountability",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Resuliency",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Honesty",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Respectfulness",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Compassion",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Integrity",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Generosity",type=(int)DDLTypeEnum.Qualities},
                    new DDL{ name="Gratitude",type=(int)DDLTypeEnum.Qualities},

                    new DDL{ name="Ask Me Personally",type=(int)DDLTypeEnum.Myproffession},
                    new DDL{ name="Student",type=(int)DDLTypeEnum.Myproffession},
                    new DDL{ name="Art/Music/Literature",type=(int)DDLTypeEnum.Myproffession},
                    new DDL{ name="Baking Finance",type=(int)DDLTypeEnum.Myproffession},
                    new DDL{ name="Technology",type=(int)DDLTypeEnum.Myproffession},
                    new DDL{ name="Administration",type=(int)DDLTypeEnum.Myproffession},
                    new DDL{ name="Enteraiment/Media",type=(int)DDLTypeEnum.Myproffession},
                    new DDL{ name="Education",type=(int)DDLTypeEnum.Myproffession},
                    new DDL{ name="Entreprenaur",type=(int)DDLTypeEnum.Myproffession},

                    new DDL{ name="Physical Type 1",type=(int)DDLTypeEnum.PhysicalType},
                    new DDL{ name="Physical Type 2",type=(int)DDLTypeEnum.PhysicalType},
                    new DDL{ name="Physical Type 3",type=(int)DDLTypeEnum.PhysicalType},
                    new DDL{ name="Physical Type 4",type=(int)DDLTypeEnum.PhysicalType},

                    new DDL{ name="Islam",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Hinduism",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Muslim",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Catholic",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Evangelical",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Spiritist",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Atheist",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Jewish",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Umbanda",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Candomble",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Afro",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="I have no religion",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="I prefer not to say",type=(int)DDLTypeEnum.Religon},
                    new DDL{ name="Indifferent",type=(int)DDLTypeEnum.Religon},

                    new DDL{ name="Writing",type=(int)DDLTypeEnum.Hobbies},
                    new DDL{ name="Painting",type=(int)DDLTypeEnum.Hobbies},
                    new DDL{ name="Doodling",type=(int)DDLTypeEnum.Hobbies},
                    new DDL{ name="Gym",type=(int)DDLTypeEnum.Hobbies},
                    new DDL{ name="Travelling",type=(int)DDLTypeEnum.Hobbies},


                    new DDL{ name="Aries",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Taurus",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Gemini",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Cancer",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Leo",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Virgo",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Libra",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Scorpio",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Sagittarius",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Capricorn",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Aquarius",type=(int)DDLTypeEnum.Sign},
                    new DDL{ name="Pisces",type=(int)DDLTypeEnum.Sign},

                    new DDL{ name="Fetch1",type=(int)DDLTypeEnum.Fetches},
                    new DDL{ name="Fetch2",type=(int)DDLTypeEnum.Fetches},
                    new DDL{ name="Fetch3",type=(int)DDLTypeEnum.Fetches},
                    new DDL{ name="Fetch4",type=(int)DDLTypeEnum.Fetches},

                };
                db.ddls.AddRange(list.Select(x => new DDL { name = x.name, type = x.type, }).ToList());
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = ResultStatus.Success.ToString() };
            }
        }



        [HttpGet]
        [Route("GetProfile")]
        public async Task<Result<EnProfile>> GetProfile(int myId,int id,bool otherProfile=false)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                try
                {
                    var list = api.GetProfile(myId, id, otherProfile);
                    return new Result<EnProfile> { Status = ResultStatus.Success, Message = "Retrieve Successfully", Data = list };
                }
                catch (Exception ex)
                {
                    return new Result<EnProfile> { Status = ResultStatus.Error, Message = ex.ToString() + "\n" + (ex.InnerException != null ? ex.InnerException.ToString() : "") };
                }
            }
        }

        
        [HttpPost]
        [Route("AddAlbum")]
        public async Task<Result> AddAlbum(EnAlbum data)
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                if (data.id == 0)
                {
                  var e=  db.albums.Add(new Album { userId = data.userId, name = data.name, timestamp = DateTime.Now, });
                    db.SaveChanges();
                    data.id = e.Entity.id;
                }
                else
                {
                    var e = db.albums.Where(x => x.id == data.id).FirstOrDefault();
                    e.name = data.name;
                    db.SaveChanges();
                }
                db.SaveChanges();
                foreach (var imgstr in data.images)
                {
                    var img = Common.Base64ToFile(_env.WebRootPath, imgstr, ".jpg");
                    if (!string.IsNullOrEmpty(img))
                    {
                        db.albumImages.Add(new AlbumImage { albumId = data.id, photo = img, timestamp = DateTime.Now, });
                        db.SaveChanges();
                    }
                }
                return new Result { Status = ResultStatus.Success, Message = "Saved Successfully" };
            }
        }
        [HttpPost]
        [Route("AddAlbumImages")]
        public async Task<Result> AddAlbumImages(EnAlbumImgObj data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                foreach (var imgstr in data.images)
                {
                    var img = Common.Base64ToFile(_env.WebRootPath, imgstr, ".jpg");
                    if (!string.IsNullOrEmpty(img))
                    {
                        db.albumImages.Add(new AlbumImage { albumId = data.id, photo = img, timestamp = DateTime.Now, });
                        db.SaveChanges();
                    }
                }
                return new Result { Status = ResultStatus.Success, Message = "Saved Successfully", };
            }
        }

        [HttpGet]
        [Route("GetAlbum")]
        public async Task<Result<EnAlbum>> GetAlbum(int myId,int userId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var list = db.albums.Where(x => x.userId == userId && ((userId == myId) || (!x.isPrivate) || x.access.Any(y => y.userId == myId))
                ).Select(x => new EnAlbum
                {
                    id = x.id,
                    userId = x.userId,
                    name = x.name,
                    isPrivate = x.isPrivate,
                    images = x.images.Select(y => y.photo).ToList()
                }).ToList();

                return new Result<EnAlbum> { Status = ResultStatus.Success, Message = "Retrieve Successfully", Data = list };
            }
        }

        [HttpPost]
        [Route("AddRating")]
        public async Task<Result> AddRating(EnRating data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.userRatings.Where(x => x.fromUserId == data.fromUserId && x.toUserId == data.toUserId).FirstOrDefault();
                if (e != null)
                {
                    e.review = data.review;
                    e.rating = data.rate;
                }
                else
                {
                    db.userRatings.Add(new UserRating { fromUserId = data.fromUserId, toUserId = data.toUserId, rating = data.rate, review = data.review, timestamp = DateTime.Now });
                }
                db.SaveChanges();
                var u = db.users.Where(x => x.id == data.toUserId).FirstOrDefault();
                var q = db.userRatings.Where(x => x.toUserId == data.toUserId);
                u.rating = q.Count() > 0 ? q.Average(x => x.rating) : 0;
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = "Saved Successfully"};
            }
        }
        [HttpGet]
        [Route("GetRating")]
        public async Task<Result<EnRatingList>> GetRating(int userId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {

                var list = api.GetRating(userId);
                return new Result<EnRatingList> { Status = ResultStatus.Success, Message = "Retrieve Successfully", Data = list };
            }
        }


        [HttpPost]
        [Route("AddReportUser")]
        public async Task<Result> AddReportUser(EnReport data)
        {
            api.AddReportUser(data);
            return new Result { Status = ResultStatus.Success, Message = "Saved Successfully" };
        }



        [HttpPost]
        [Route("AddDiary")]
        public async Task<Result> AddDiary(EnDiary data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if (!string.IsNullOrEmpty(data.photo))
                {
                    data.photo = Common.Base64ToFile(_env.WebRootPath, data.photo, ".jpg");
                }
                db.diaries.Add(new Diary { dateTime = data.date, title = data.title, description = data.content, photo = data.photo, userId = data.userId });
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = "Saved Successfully" };
            }
        }
        [HttpGet]
        [Route("GetDiary")]
        public async Task<Result<EnDiary>> GetDiary(int userId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {

                var list = db.diaries.Where(x => x.userId == userId).Select(x => new EnDiary
                {
                    userId = userId,
                    photo = x.photo,
                    title = x.title,
                    content = x.description,
                    date = x.dateTime
                }).ToList();
                return new Result<EnDiary> { Status = ResultStatus.Success, Message = "Retrieve Successfully", Data = list };
            }
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<Result> ForgotPassword(string email)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.users.Where(x => !x.isdeleted && x.roleId == (int)RoleEnum.User && x.source == (int)UserSourceTypeEnum.Internal && x.email == email).FirstOrDefault();
                if (e != null)
                {
                    string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                    var code = "7777";
                    var template = System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "email_templates", "forgot_password.html"));
                    template = template.Replace("@{{name}}", e.name);
                    template = template.Replace("@{{code}}", code);
                    db.verificationCodes.Add(new VerificationCode { code = code, timestamp = DateTime.Now, userId = e.id, });
                    db.SaveChanges();
                    return new Result { Status = ResultStatus.Success, Message = "Reset Code Sent On Email", Data = e.id };
                }
                else
                {
                    return new Result { Status = ResultStatus.Error, Message = "Invalid Email", };
                }
            }
        }
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<Result> ResetPassword(string email,string code, string password)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.users.Where(x => !x.isdeleted && x.roleId == (int)RoleEnum.User && x.source == (int)UserSourceTypeEnum.Internal && x.email == email).FirstOrDefault();
                if (e != null)
                {
                    var c = db.verificationCodes.Where(x => x.userId == e.id && x.code == code).FirstOrDefault();
                    if (c != null)
                    {
                        e.password = password;
                        db.verificationCodes.Remove(c);
                        db.SaveChanges();
                        return new Result { Status = ResultStatus.Success, Message = "Password Reset Successfully", };
                    }
                }
                return new Result { Status = ResultStatus.Error, Message = "Invalid Code", };
            }
        }
        [HttpPost]
        [Route("AddFirebaseToken")]
        public async Task<Result> AddFirebaseToken(EnFirebaseToken data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                try
                {
                    db.firebaseTokens.Add(new FirebaseToken { device = data.device, os = data.os, token = data.token, userId = data.userId, timestamp = DateTime.Now });                 
                    db.SaveChanges();
                    return new Result { Status = ResultStatus.Success, Message = "Saved Successfully" };
                }
                catch (Exception ex)
                {
                    return new Result { Status = ResultStatus.Error, Message = "Unable to add Firebase Token", };
                }
            }
        }
        [HttpPost]
        [Route("RemoveFirebaseToken")]
        public async Task<Result> RemoveFirebaseToken(int userId,string device)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                try
                {
                    db.firebaseTokens.RemoveRange(db.firebaseTokens.Where(x => x.userId == userId && x.device == device).ToList());
                    db.SaveChanges();
                    return new Result { Status = ResultStatus.Success, Message = "Deleted Successfully" };
                }
                catch (Exception ex)
                {
                    return new Result { Status = ResultStatus.Error, Message = "Unable to delete Firebase Token", };
                }
            }
        }

        [HttpPost]
        [Route("ReadChat")]
        public async Task<Result> ReadChat(int fromUserId,int toUserId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var cgm = db.chatThreads.Where(x => (x.user1Id == fromUserId || x.user1Id == toUserId) && (x.user2Id == fromUserId || x.user2Id == toUserId)).FirstOrDefault();

                if (cgm != null)
                {
                    if (fromUserId == cgm.user1Id)
                    {
                        cgm.user1_unread =0;
                    }
                    else
                    {
                        cgm.user2_unread = 0;
                    }
                    db.SaveChanges();
                }
                return new Result { Status = ResultStatus.Success, Message = "Updated Successfully" };
            }
        }
        [HttpGet]
        [Route("GetChatList")]
        public async Task<Result<EnChat>> GetChatList(int userId)
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                var list = db.chatThreads.Where(x => x.user1Id == userId || x.user2Id ==userId).Select(x => new EnChat
                {
                    userId = x.user1Id == userId ? x.user2Id : x.user1Id,
                    name = x.user1Id == userId ? x.user2.name : x.user1.name,
                    photo = x.user1Id == userId ? x.user2.photo : x.user1.photo,                 
                    timeStamp = x.last_message_timestamp,
                    unreadCount = x.user1Id== userId ?x.user1_unread:x.user2_unread,
                    lastMessage = x.last_message,
                    messageType = x.message_type,
                    duration = x.additional_info,
                }).OrderByDescending(x => x.timeStamp).ToList();
                return new Result<EnChat> { Status = ResultStatus.Success, Message = "Retrieve Successfully", Data = list };
            }
        }
        [HttpGet]
        [Route("GetMessageList")]
        public async Task<Result<EnChatMessage>> GetMessageList(int userId1, int userId2)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var chatThreadId = db.chatThreads.Where(x => (x.user1Id == userId1 || x.user1Id == userId2) && (x.user2Id == userId1 || x.user2Id == userId2)).Select(x => x.id).FirstOrDefault();

                var list = db.chats.Where(x => x.chatThreadId== chatThreadId).Select(x => new EnChatMessage
                {
                    attachment = x.attachment,
                    messageType = x.messageType,
                    message = x.message,
                    timeStamp = x.timestamp,
                    duration = x.duration,
                    senderId=x.senderId,
                    senderName = x.sender.name,
                    senderhoto = x.sender.photo,              
                    id = x.id
                }).ToList();
                return new Result<EnChatMessage> { Status = ResultStatus.Success, Message = "Retrieve Successfully", Data = list };
            }
        }
        //[HttpGet]
        //[Route("TestFirebase")]
        //public async Task<Result> TestFirebase(string token,string messsage)
        //{
        //   NotificationModel n = new NotificationModel();
        //    n.PushNotificationToAndroid(new List<EnDeviceToken> { new EnDeviceToken {
        // TokenID=token,
        //} }, 1, 1, messsage, 1, "Test", 0
        //        );
        //    return new Result { Status = ResultStatus.Success, };
        //}
        [HttpPost]
        [Route("AddChatMessage")]
        public async Task<Result> AddChatMessage(EnChatMessageSend message)
        {
            ChatModel model = new ChatModel(this.dbOption, this._configuration,this._hubcontext);
            model.SaveEmployeeChat(message.fromId, message.toId, message.message, message.type);
            return new Result { Status = ResultStatus.Success, Message = "Send Successfully" };


        }
        [HttpPost]
        [Route("UpdateNotificationFlag")]
        public async Task<Result> UpdateNotificationFlag(EnNotificationFlag data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                foreach (var item in db.firebaseTokens.Where(x => x.userId == data.userId && x.device == data.device).ToList())
                {
                    item.isNotificationFlag = data.isNotificationFlag;
                }
                db.SaveChanges();
            }
            return new Result { Status = ResultStatus.Success, Message = "Updated Successfully" };
        }


        [HttpPost]
        [Route("CallDial")]
        public async Task<Result<EnAgoraToken>> CallDial(int  fromUserId,int toUserId,bool isVideo=false)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                try
                {
                    NotificationModel n = new NotificationModel();
                    var fbtokens = db.firebaseTokens.Where(x => x.userId == toUserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();

                    //if (fbtokens.Count() == 0)
                    //{
                    //    return new Result<EnAgoraToken> { Status = ResultStatus.Error, Message = "User Not Avaliable" };
                    //}
                    var agtoken = api.GenerateAgoraToken();
                    var user = db.users.Where(x => x.id == fromUserId).FirstOrDefault();
                    var e = db.callLogs.Add(new CallLog { isVideo = isVideo, channel = agtoken.channel, token = agtoken.token, fromUserId = fromUserId, timestamp = DateTime.Now, toUserId = toUserId });
                    db.SaveChanges();
                    agtoken.callId = e.Entity.id;
                    agtoken.isVideo = isVideo;
                    var message = "";
                    var noti = new EnNotificatoinPayload
                    {
                        notificationType = (int)NotificationTypeEnum.ReceiveCall,
                        referenceId = fromUserId,
                        senderName = user.name,
                        senderPhoto = user.photo,
                        agoraToken = agtoken,
                    };
                    n.PushNotificationToAndroid(fbtokens, "Receive New Call", message, noti);

                    var uids = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == toUserId).Select(c => c.ConnectionId).ToList();
                    await this._hubcontext.Clients.Clients(uids).SendAsync("ReceiveCall", Common.Serialize(noti));
                    return new Result<EnAgoraToken> { Status = ResultStatus.Success, Message = "Request Sent Successfully", Data = new List<EnAgoraToken> { agtoken } };
                }
                catch(Exception ex)
                {
                    return new Result<EnAgoraToken> { Status = ResultStatus.Error, Message = ex.ToString() };
                }
            }            
        }


        [HttpPost]
        [Route("CallRinging")]
        public async Task<Result> CallRinging(int myId,int callId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {

                var call = db.callLogs.Where(x => x.id == callId).FirstOrDefault();
                int secondUserId = call.fromUserId == myId ? call.toUserId : call.fromUserId;
                NotificationModel n = new NotificationModel();
                var fbtokens = db.firebaseTokens.Where(x => x.userId == secondUserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();

                var user = db.users.Where(x => x.id == myId).FirstOrDefault();
                var message = "Call Ringing";
                var noti = new EnNotificatoinPayload
                {
                    notificationType = (int)NotificationTypeEnum.RingingCall,
                    referenceId = myId,
                    senderName = user.name,
                    senderPhoto = user.photo,
                    agoraToken = new EnAgoraToken {isVideo=call.isVideo, callId = call.id, token = call.token, channel = call.channel }
                };
                n.PushNotificationToAndroid(fbtokens, "Call Ringing", message, noti);

                var uids = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == secondUserId).Select(c => c.ConnectionId).ToList();
                await this._hubcontext.Clients.Clients(uids).SendAsync("RingingCall", Common.Serialize(noti));

                return new Result { Status = ResultStatus.Success, Message = "Request Sent Successfully", };
            }
        }
        [HttpPost]
        [Route("CallIamBusy")]
        public async Task<Result> CallIamBusy(int myId, int callId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var call = db.callLogs.Where(x => x.id == callId).FirstOrDefault();
                int secondUserId = call.fromUserId == myId ? call.toUserId : call.fromUserId;
                NotificationModel n = new NotificationModel();
                var fbtokens = db.firebaseTokens.Where(x => x.userId == secondUserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();

                var user = db.users.Where(x => x.id == myId).FirstOrDefault();
                var message = $"{user.name} not avaliable try later";
                var noti = new EnNotificatoinPayload
                {
                    notificationType = (int)NotificationTypeEnum.UserBusy,
                    referenceId = myId,
                    senderName = user.name,
                    senderPhoto = user.photo,
                    agoraToken = new EnAgoraToken { isVideo = call.isVideo, callId = call.id, token = call.token, channel = call.channel }
                };
                n.PushNotificationToAndroid(fbtokens, "User Busy", message, noti);


                var uids = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == secondUserId).Select(c => c.ConnectionId).ToList();
                await this._hubcontext.Clients.Clients(uids).SendAsync("UserBusy", Common.Serialize(noti));

                return new Result { Status = ResultStatus.Success, Message = "Request Sent Successfully",  };
            }
        }
        [HttpPost]
        [Route("CallAccepted")]
        public async Task<Result> CallAccepted(int myId, int callId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var call = db.callLogs.Where(x => x.id == callId).FirstOrDefault();
                int secondUserId = call.fromUserId == myId ? call.toUserId : call.fromUserId;
                NotificationModel n = new NotificationModel();
                var fbtokens = db.firebaseTokens.Where(x => x.userId == secondUserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();

                var user = db.users.Where(x => x.id == myId).FirstOrDefault();
                var message = $"{user.name} accepted call";
                var noti = new EnNotificatoinPayload
                {
                    notificationType = (int)NotificationTypeEnum.Accepted,
                    referenceId = myId,
                    senderName = user.name,
                    senderPhoto = user.photo,
                    agoraToken = new EnAgoraToken { isVideo = call.isVideo, callId = call.id, token = call.token, channel = call.channel }
                };
                n.PushNotificationToAndroid(fbtokens, "Call Accepted", message, noti);



                var uids = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == secondUserId).Select(c => c.ConnectionId).ToList();
                await this._hubcontext.Clients.Clients(uids).SendAsync("Accepted", Common.Serialize(noti));

                return new Result { Status = ResultStatus.Success, Message = "Request Sent Successfully"};
            }
        }
        [HttpPost]
        [Route("CallRejected")]
        public async Task<Result> CallRejected(int myId, int callId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var call = db.callLogs.Where(x => x.id == callId).FirstOrDefault();
                int secondUserId = call.fromUserId == myId ? call.toUserId : call.fromUserId;
                NotificationModel n = new NotificationModel();
                var fbtokens = db.firebaseTokens.Where(x => x.userId == secondUserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();

                var user = db.users.Where(x => x.id == myId).FirstOrDefault();
                var message = $"{user.name} rejected call";
                var noti = new EnNotificatoinPayload
                {
                    notificationType = (int)NotificationTypeEnum.Rejected,
                    referenceId = myId,
                    senderName = user.name,
                    senderPhoto = user.photo,
                    agoraToken = new EnAgoraToken { isVideo = call.isVideo, callId = call.id, token = call.token, channel = call.channel }
                };
                n.PushNotificationToAndroid(fbtokens, "Call Rejected", message,noti);




                var uids = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == secondUserId).Select(c => c.ConnectionId).ToList();
                await this._hubcontext.Clients.Clients(uids).SendAsync("Rejected", Common.Serialize(noti));

                return new Result { Status = ResultStatus.Success, Message = "Request Sent Successfully"};
            }
        }
        [HttpPost]
        [Route("CallEnded")]
        public async Task<Result> CallEnded(int myId, int callId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var call = db.callLogs.Where(x => x.id == callId).FirstOrDefault();
                int secondUserId = call.fromUserId == myId ? call.toUserId : call.fromUserId;
                NotificationModel n = new NotificationModel();
                var fbtokens = db.firebaseTokens.Where(x => x.userId == secondUserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();

                var user = db.users.Where(x => x.id == myId).FirstOrDefault();
                var message = $"{user.name} ended call";
                var noti = new EnNotificatoinPayload
                {
                    notificationType = (int)NotificationTypeEnum.CallEnded,
                    referenceId = myId,
                    senderName = user.name,
                    senderPhoto = user.photo,
                    agoraToken = new EnAgoraToken { isVideo = call.isVideo, callId = call.id, token = call.token, channel = call.channel }
                };
                n.PushNotificationToAndroid(fbtokens, "Call Ended", message, noti);

                var uids = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == secondUserId).Select(c => c.ConnectionId).ToList();
                await this._hubcontext.Clients.Clients(uids).SendAsync("CallEnded", Common.Serialize(noti));

                return new Result { Status = ResultStatus.Success, Message = "Request Sent Successfully",  };
            }
        }

        [HttpPost]
        [Route("AddStory")]
        public async Task<Result> AddStory(EnStory data)
        {
            using (DBContext db=new DBContext(this.dbOption))
            {
                foreach (var item in data.photos)
                {
                    item.photo = Common.Base64ToFile(_env.WebRootPath, item.photo, ".jpg");
                }

                var storyId = db.stories.Where(x => x.userId == data.userId).Select(x => x.id).FirstOrDefault();
                if (storyId == 0)
                {
                    db.stories.Add(new Story
                    {
                        timestamp = DateTime.Now,
                        userId = data.userId,
                        photos = data.photos.Select(x => new StoryPhoto { content = x.content, photo = x.photo }).ToList()
                    });
                    db.SaveChanges();
                }
                else
                {
                    db.storyPhotos.AddRange(data.photos.Select(x => new StoryPhoto { photo = x.photo, storyId = storyId, content = x.content }).ToList());
                    db.SaveChanges();
                }
                return new Result { Status = ResultStatus.Success, Message = "Saved Successfully", };
            }
        }
        [HttpGet]
        [Route("GetStories")]
        public async Task<Result<EnStory>> GetStories()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var list = api.GetStories();
                return new Result<EnStory> { Status = ResultStatus.Success, Message = "Reterive Successfully", Data = list };
            }
        }

        [HttpPost]
        [Route("UpdateAlbumIsPrivate")]
        public async Task<Result> UpdateAlbumIsPrivate(int id, bool isPrivate)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var model = db.albums.Where(x =>  x.id == id).FirstOrDefault();
                model.isPrivate = isPrivate;
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = "Saved Successfully" };
            }
        }
        [HttpGet]
        [Route("GenerateAlbumCode")]
        public async Task<Result> GenerateAlbumCode(int userId,int id)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {

                Random generator = new Random();
                String r = generator.Next(0, 1000000).ToString("D6");

                while (db.hiddenAlbums.Any(x => x.code == r && x.album.userId == userId))
                {
                    r = generator.Next(0, 1000000).ToString("D6");
                }
                db.hiddenAlbums.Add(new HiddenAlbum { timestamp = DateTime.Now, albumId = id, code = r, status = (int)HiddenAlbumCodeStatus.NotUsed });
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = "Saved Successfully", Data = r };
            }
        }
        [HttpPost]
        [Route("ApplyAlbumCode")]
        public async Task<Result> ApplyAlbumCode(int myUserId,int userId, string code)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.hiddenAlbums.Where(x => x.album.userId == userId && x.code == code && x.status == (int)HiddenAlbumCodeStatus.NotUsed).FirstOrDefault();
                if (e != null)
                {
                    e.status = (int)HiddenAlbumCodeStatus.Used;
                    e.userId = myUserId;
                    db.SaveChanges();
                    return new Result { Status = ResultStatus.Success, Message = "Saved Successfully" };
                }
                else
                {
                    db.SaveChanges();
                    return new Result { Status = ResultStatus.Error, Message = "Invalid Code", };
                }

            }
        }




        [HttpPost]
        [Route("CreateStreaming")]
        public async Task<Result<EnStreamingDetail>> CreateStreaming(EnStreaming data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                data.banner = Common.Base64ToFile(_env.WebRootPath, data.banner, ".jpg");

                var agtoken = api.GenerateAgoraToken();

                var e = db.streamings.Add(new Streaming { createdOn = DateTime.Now, banner = data.banner, title = data.title, userId = data.userId, category = data.category, token = agtoken.token, channel = agtoken.channel });
                db.SaveChanges();
                var u = db.users.Where(x => x.id == data.userId).FirstOrDefault();
                return new Result<EnStreamingDetail>
                {
                    Status = ResultStatus.Success,
                    Message = "Stream Created",
                    Data = new List<EnStreamingDetail>
                    {

                        new EnStreamingDetail{
                            username= u.name,photo=u.photo,
                            banner=data.banner, category=data.category, title=data.title,  token=agtoken.token, channel=agtoken.channel, userId=data.userId, id=e.Entity.id
                        }
                    }
                };
            }
        }

        [HttpPost]
        [Route("StartStream")]
        public async Task<Result> StartStream(int id)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.streamings.Where(x => x.id == id).FirstOrDefault();
                e.status = (int)StreamStatus.Started;
                e.startedOn = DateTime.Now;
                db.SaveChanges();
                //firenewstream notification
                return new Result { Status = ResultStatus.Success, Message = "Stream Started" };
            }         
        }
        [HttpPost]
        [Route("EndStream")]
        public async Task<Result> EndStream(int id)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.streamings.Where(x => x.id == id).FirstOrDefault();
                e.status = (int)StreamStatus.Ended;
                e.endedOn = DateTime.Now;
                db.SaveChanges();
                //fire notification
                return new Result { Status = ResultStatus.Success, Message = "Stream Started" };
            }
        }
        [HttpPost]
        [Route("JoinStream")]
        public async Task<Result<EnStreamingDetail>> JoinStream(int id,int userId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var e = db.streamingMembers.Where(x => x.id == id && x.userId == userId).FirstOrDefault();
                if (e == null)
                {
                    db.streamingActivities.Add(new StreamingActivity { userId = userId, streamId = id, type = (int)StreamingActivityEnum.View, timestamp = DateTime.Now, });
                    db.streamingMembers.Add(new StreamingMember { userId = userId, streamId = id, timestamp = DateTime.Now, });
                    db.SaveChanges();
                }

                //fire notification
                var isLike = db.streamingActivities.Any(x => x.streamId == id && x.userId == userId && x.type == (int)StreamingActivityEnum.Like);
             var st = db.streamings.Where(x => x.id == id).Include(x => x.user).FirstOrDefault();
                return new Result<EnStreamingDetail>
                {
                    Data = new List<EnStreamingDetail>
                    {
                        new EnStreamingDetail{
                            likeByMe=isLike,
                            username= st.user.name,photo=st.user.photo,
                            banner=st.user.banner, category=st.category, title=st.title,  token=st.token, channel=st.channel, userId=st.userId, id=st.id
                        }
                    }
                };
            }
        }

        [HttpPost]
        [Route("LikeStream")]
        public async Task<Result> LikeStream(EnStreamAddLike data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                if (!data.isLike)
                {
                    db.streamingActivities.RemoveRange(db.streamingActivities.Where(x => x.streamId == data.streamId && x.userId == data.userId && x.type==(int)StreamingActivityEnum.Like).ToList());
                    db.SaveChanges();
                }
                else
                {
                    var st = db.streamingActivities.Where(x => x.streamId == data.streamId && x.userId == data.userId && x.type == (int)StreamingActivityEnum.Like).FirstOrDefault();
                    if (st != null)
                    {
                        db.streamingActivities.Add(new StreamingActivity { timestamp = DateTime.Now, streamId = data.streamId, type = (int)StreamingActivityEnum.Like, userId = data.userId, });
                        db.SaveChanges();
                    }
                }                
                return new Result { Status = ResultStatus.Success, Message = "Success", };
            }
        }

        [HttpPost]
        [Route("CommentOnStream")]
        public async Task<Result> CommentOnStream(EnStreamAddComment data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                db.streamingActivities.Add(new StreamingActivity { timestamp = DateTime.Now, streamId = data.streamId, detail = data.comment, type = (int)StreamingActivityEnum.Comment, userId = data.userId, });
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = "Success", };
            }
        }
        [HttpPost]
        [Route("CommentReplyOnStream")]
        public async Task<Result> CommentReplyOnStream(EnStreamAddSubComment data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                db.streamingActivities.Add(new StreamingActivity {parentId=data.commentId, timestamp = DateTime.Now, streamId = data.streamId, detail = data.comment, type = (int)StreamingActivityEnum.Comment, userId = data.userId, });
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = "Success", };
            }
        }
        [HttpPost]
        [Route("GetStreamComments")]
        public async Task<Result<EnStreamComment>> GetStreamComments(int streamId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var list = db.streamingActivities.Where(x =>!x.parentId.HasValue && x.streamId == streamId && x.type == (int)StreamingActivityEnum.Comment).Select(x => new EnStreamComment
                {
                     id=x.id, 
                    comment = x.detail,
                    timestamp = x.timestamp,
                    name = x.user.name,
                    photo = x.user.photo
                }).ToList();
                db.SaveChanges();
                return new Result<EnStreamComment> { Status = ResultStatus.Success, Message = "Success", Data = list };
            }
        }
        [HttpGet]
        [Route("GetStreamStats")]
        public async Task<Result<EnStreamStats>> GetStreamStats(int streamId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var model = new EnStreamStats
                {
                    likes = db.streamingActivities.Where(x => x.streamId == streamId && x.type == (int)StreamingActivityEnum.Like).Count(),
                    views = db.streamingActivities.Where(x => x.streamId == streamId && x.type == (int)StreamingActivityEnum.View).Count()
                };
                db.SaveChanges();
                return new Result<EnStreamStats> { Status = ResultStatus.Success, Message = "Success", Data = new List<EnStreamStats> { model } };
            }
        }
        [HttpGet]
        [Route("GetStreams")]
        public async Task<Result<EnStreamList>> GetStreams(int userId)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
            var list=   db.streamings.Where(x => x.status == (int)StreamStatus.Started).Select(x => new EnStreamList
                {
                    id = x.id,
                    title = x.title,
                    category = x.category,
                    banner = x.banner,
                }).ToList();
                return new Result<EnStreamList> { Status = ResultStatus.Success, Message = "Success", Data = list };
            }
        }

        [HttpPost]
        [Route("AddErrorLog")]
        public async Task<Result> AddErrorLog(ErrorLog data)
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                data.timestamp = DateTime.Now;
                db.errorlog.Add(data);
                db.SaveChanges();
                return new Result { Status = ResultStatus.Success, Message = "Success" };
            }
        }
        [HttpPost]
        [Route("GetErrorLog")]
        public async Task<Result<ErrorLog>> GetErrorLog()
        {
            using (DBContext db = new DBContext(this.dbOption))
            {
                var list = db.errorlog.ToList();
                return new Result<ErrorLog> { Status = ResultStatus.Success, Message = "Success", Data = list };
            }
        }

    }
}
