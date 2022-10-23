using james.ChatR;
using james.Helpers.Custom;
using james.Helpers.Custom.Api;
using james.Helpers.General;
using james.Models.DB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace james.Models
{
    public class ApiModel
    {
        DBContext db;
        IHubContext<ChatHub> _hubcontext;
        public ApiModel(DbContextOptions<DBContext> _dbOptions, IHubContext<ChatHub> hubcontext)
        {
            this._hubcontext = hubcontext;
            db = new DBContext(_dbOptions);
        }
        public EnAgoraToken GenerateAgoraToken()
        {
            var channelName = DateTime.Now.ToString("yyyyMMddHHmmss");

            string URL = $"http://att.pixtechcreation.com:8080/access_token?channelName={channelName}&role=subscriber&expireTime=6400";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.ContentType = "application/json; charset=utf-8";            
            request.PreAuthenticate = true;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                var json = reader.ReadToEnd();
               var result= Common.Deserialize<EnAgoraToken>(json);
                result.channel = channelName;
                return result;
            }

        }
        public List<EnUser> GetUsers(EnFilter filter)
        {
            filter.lookingforRelation = filter.lookingforRelation ?? new List<int> { };
            filter.relationStatus = filter.relationStatus ?? new List<int> { };

            var coord = db.users.Where(x => x.id == filter.myId).Select(x => new { x.lat, x.lng }).FirstOrDefault();
            filter.lat = filter.lat ?? coord.lat;
            filter.lng = filter.lng ?? coord.lng;
            var rptUserIds = db.reportUsers.Where(x => x.fromUserId == filter.myId).Select(x => x.toUserId).ToList();
            var skipUids = db.matches.Where(x => x.fromUserId == filter.myId && (x.type == (int)MatchTypeEnum.SuperLike || x.type == (int)MatchTypeEnum.Like || x.type == (int)MatchTypeEnum.Dislike)).Select(x => x.toUserId).ToList();
            skipUids.AddRange(rptUserIds);
            var users = db.users.Where(x => !x.isdeleted && x.roleId == (int)RoleEnum.User && x.id != filter.myId && !skipUids.Contains(x.id)
            && (!filter.startAge.HasValue || !filter.EndAge.HasValue || (x.age >= filter.startAge && x.age <= filter.EndAge))
            && (filter.lookingforRelation.Count() == 0 || x.lookingRelations.Any(y => filter.lookingforRelation.Contains(y.relationId)))
            && (filter.relationStatus.Count() == 0 || (x.relationshipStatusId.HasValue && filter.relationStatus.Contains(x.relationshipStatusId.Value)))
            && (string.IsNullOrEmpty(filter.name) || x.name.Contains(filter.name))
            && (!filter.lookingForGender.HasValue || x.genderId == filter.lookingForGender)
            && (string.IsNullOrEmpty(filter.profession) || x.profession.Contains(filter.profession))
             && (!filter.children.HasValue || x.childrenId == filter.children)
             && (!filter.smoke.HasValue || x.smokeId == filter.smoke)
             && (!filter.sexualOrientation.HasValue || x.sexualOrientationId == filter.sexualOrientation)
             && (!filter.bodyArt.HasValue || x.physicalTypeId == filter.bodyArt)
             && (!filter.religion.HasValue || x.religionId == filter.religion)
            ).Include(x => x.vaccine).Include(x => x.lookingRelations).ThenInclude(x => x.relation)
                .Select(x => new EnUser
                {
                    id = x.id,
                    photo = x.photo,
                    name = x.name,
                    age = !x.hideage ? x.age : default(int?),
                    likes = x.likes,
                    rating = x.rating,
                    eduction = x.eduction,
                    profession = x.profession,
                    relation = x.lookingRelations.Select(y => y.relation.name).FirstOrDefault(),
                    vaccine = x.vaccineId.HasValue ? x.vaccine.name : "",
                    distance = x.gelocationBydistance == 1 ? Common.GetDistance(x.lat, x.lng, filter.lat, filter.lng) : default(double?)
                }).OrderByDescending(x => x.id).ToList();
            return users;
        }

      public  List<EnReportUserOptionList> GetReportUserList()
        {
            var reaons = new List<EnReportUserOptionList>
            {
                new EnReportUserOptionList{name="It's spam" },
                new EnReportUserOptionList{name= "Nudity or sexual activity"},
                new EnReportUserOptionList{name= "I just don't like it"},
                new EnReportUserOptionList{name= "Hate speech or symbol"},
                new EnReportUserOptionList{name= "Bullying or harassment"},
                new EnReportUserOptionList{name= "Violence or dangerous organizations"},
                new EnReportUserOptionList{name= "False information"},
                new EnReportUserOptionList{name= "Scan or fraud"},
                new EnReportUserOptionList{name= "Suicide or sell-injury"},
                new EnReportUserOptionList{name= "Sales of illegal or regulated goods"},
                new EnReportUserOptionList{name= "Intellectual property violation"},
                new EnReportUserOptionList{name= "Eating disorder"}
            };
            return reaons;
        }

        public async Task<Result> UpdateLike(int fromUserId, int toUserId)
        {
            db.matches.Add(new Match { fromUserId = fromUserId, toUserId = toUserId, timestamp = DateTime.Now, type = (int)MatchTypeEnum.Like });
            
            var u = db.users.Where(x => x.id == toUserId).FirstOrDefault();
            u.likes += 1;
            db.SaveChanges();
           await MakeMatch(fromUserId, toUserId);
            return new Result { Status = ResultStatus.Success, Message = "Liked", };       
        }
        public async Task<Result> UpdateSuperLike(int fromUserId, int toUserId)
        {
            db.matches.Add(new Match { fromUserId = fromUserId, toUserId = toUserId, timestamp = DateTime.Now, type = (int)MatchTypeEnum.SuperLike });

            var u = db.users.Where(x => x.id == toUserId).FirstOrDefault();
            u.likes += 1;
            db.SaveChanges();
           await MakeMatch(fromUserId, toUserId);
            return new Result { Status = ResultStatus.Success, Message = "Super Liked", };
        }
        public async Task MakeMatch(int fromUserId, int toUserId)
        {
            var like1 = db.matches.Any(x => x.fromUserId == fromUserId && x.toUserId == toUserId && (x.type == (int)MatchTypeEnum.Like || x.type == (int)MatchTypeEnum.SuperLike));
            var like2 = db.matches.Any(x => x.fromUserId == toUserId && x.toUserId == fromUserId && (x.type == (int)MatchTypeEnum.Like || x.type == (int)MatchTypeEnum.SuperLike));
            if (like1 && like2)
            {
                db.matches.Add(new Match { type = (int)MatchTypeEnum.Match, timestamp = DateTime.Now, fromUserId = fromUserId, toUserId = toUserId, });
                db.chatThreads.Add(new ChatThread { user1Id = fromUserId, user2Id = toUserId, last_message_timestamp = DateTime.Now, });
                db.SaveChanges();


                NotificationModel n = new NotificationModel();
                var to = db.users.Where(x => x.id == toUserId).Select(x => x).FirstOrDefault();
                var from = db.users.Where(x => x.id == fromUserId).Select(x => x).FirstOrDefault();
                {
                    var tokens = db.firebaseTokens.Where(x => x.userId == fromUserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();
                    var noti = new EnNotificatoinPayload
                    {
                        referenceId = toUserId,
                        notificationType = (int)NotificationTypeEnum.MatchFound,
                        senderName = to.name,
                        senderPhoto = to.photo,
                    };
                    n.PushNotificationToAndroid(tokens, "New Match", $"You have new match {to.name}", noti);


                    var uids = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == fromUserId).Select(c => c.ConnectionId).ToList();
                    await this._hubcontext.Clients.Clients(uids).SendAsync("MatchFound", Common.Serialize(noti));
                }
                {
                    var tokens = db.firebaseTokens.Where(x => x.userId == toUserId).Select(x => new EnDeviceToken { TokenID = x.token, isNotificationFlag = x.isNotificationFlag }).ToList();
                    var noti = new EnNotificatoinPayload
                    {
                        referenceId = fromUserId,
                        notificationType = (int)NotificationTypeEnum.MatchFound,
                        senderName = from.name,
                        senderPhoto = from.photo
                    };
                    n.PushNotificationToAndroid(tokens, "New Match", $"You have new match {from.name}", noti);


                    var uids = (ChatHub.ConnectedUsers ?? new List<EnChatUser> { }).Where(x => x.EmpId == toUserId).Select(c => c.ConnectionId).ToList();
                    await this._hubcontext.Clients.Clients(uids).SendAsync("MatchFound", Common.Serialize(noti));

                }
            }
        }
        public Result UpdateProfileView(int userId)
        {
            var u = db.users.Where(x => x.id == userId).FirstOrDefault();
            u.views += 1;
            db.SaveChanges();
            return new Result { Status = ResultStatus.Success, Message = "Super Liked", };
        }

        public Result UpdateDisLike(int fromUserId, int toUserId)
        {
            db.matches.Add(new Match { fromUserId = fromUserId, toUserId = toUserId, timestamp = DateTime.Now, type = (int)MatchTypeEnum.Dislike });
            db.SaveChanges();
            return new Result { Status = ResultStatus.Success, Message = "DisLiked", };
        }
        public EnProfileStats GetProfileStats(int id)
        {
            EnProfileStats result = new EnProfileStats { };
            result.views = db.users.Where(x => x.id == id).Select(x => x.views).FirstOrDefault();
            result.likes = db.matches.Where(x => x.toUserId == id && x.type == (int)MatchTypeEnum.Like).Count();
            result.superlikes = db.matches.Where(x => x.toUserId == id && x.type == (int)MatchTypeEnum.SuperLike).Count();
            result.dislikes = db.matches.Where(x => x.toUserId == id && x.type == (int)MatchTypeEnum.Dislike).Count();
            return result;
        }
        public List<EnProfile> GetProfile(int myId, int id, bool otherProfile = false)
        {
            if (myId != id)
            {
                UpdateProfileView(id);
            }
            var ismatch=db.matches.Any(x => x.type == (int)MatchTypeEnum.Match && ((x.fromUserId == myId && x.toUserId == id) || (x.fromUserId == id && x.toUserId == myId)));
            var list = db.users.Where(x => x.id == id).Select(data => new EnProfile
            {
                id = data.id,
                aboutMe = data.aboutMe,
                age = data.age,
                email = data.email,
                alcoholConsumption = data.alcoholConsumptionId.HasValue ? data.alcoholConsumption.name : null,
                annualIncome = data.annualIncomeId.HasValue ? data.annualIncome.name : null,
                children = data.childrenId.HasValue ? data.children.name : null,
                fetiches = data.fetichesId.HasValue ? data.fetiches.name : null,
                gender = data.genderId.HasValue ? data.gender.name : null,
                height = data.height,
                lookingGender = data.lookingGenderId.HasValue ? data.lookingGender.name : null,
                myProfession = data.myProfessionId.HasValue ? data.myProfession.name : null,
                name = data.name,
                physicalType = data.physicalTypeId.HasValue ? data.physicalType.name : null,
                relationshipStatus = data.relationshipStatusId.HasValue ? data.relationshipStatus.name : null,
                zipcode = data.zipcode,
                whereamiknow = data.whereamiknow.HasValue ? (data.whereamiknow.Value == 1 ? "Yes" : "No") : null,
                vaccine = data.vaccineId.HasValue ? data.vaccine.name : null,
                smoke = data.smokeId.HasValue ? data.smoke.name : null,
                username = data.username,
                sign = data.signId.HasValue ? data.sign.name : null,
                religion = data.religionId.HasValue ? data.religion.name : null,
                sexualOrientation = data.sexualOrientationId.HasValue ? data.sexualOrientation.name : null,
                photo = data.photo,
                eduction = data.eduction,
                hideage = data.hideage,
                ismatch = ismatch,
                last_relationship = data.last_relationship,
                gelocationBydistance = data.gelocationBydistance.HasValue ? (data.gelocationBydistance.Value == 1 ? "Yes" : "No") : null,
                lookingRelations = data.lookingRelations.Select(y => y.relation.name).ToList(),
                personalities = data.personalities.Select(y => y.personality.name).ToList(),
                hobbies = data.hobbies.Select(y => y.hobby.name).ToList(),
                qualities = data.qualities.Select(y => y.quality.name).ToList(),
                likes = data.likes,
                rating = data.rating,
                looking_for = data.lookingRelations.Select(y => y.relation.name).FirstOrDefault(),
                city = data.city,
                lat = data.lat,
                lng = data.lng,
                albums = data.albums.Where(x => (id == myId) || (!x.isPrivate) || x.access.Any(y => y.userId == myId)).Select(x => new EnAlbum
                {
                     id=x.id,
                    name = x.name,
                    isPrivate = x.isPrivate,
                    images = x.images.Select(y => y.photo).ToList(),
                }).ToList(),
                ratings = data.ratings.Select(x => new EnRatingList
                {
                    name = x.fromUser.name,
                    review = x.review,
                    rate = x.rating,
                    photo = x.fromUser.photo,
                    timestamp = x.timestamp,
                }).ToList(),

            }).ToList();
            var myCoord = db.users.Where(x => x.id == myId).Select(x => new { x.lat, x.lng }).FirstOrDefault();
            foreach (var item in list.Where(x => x.lat != null && x.lng != null))
            {
                item.distance = Common.GetDistance(myCoord.lat, myCoord.lng, item.lat, item.lng);
            }
            return list;
        }
       
        public List<EnRatingList> GetRating(int userId)
        {
                var list = db.userRatings.Where(x => x.toUserId == userId).Select(x => new EnRatingList
                {
                    name = x.fromUser.name,
                    review = x.review,
                    rate = x.rating,
                    photo = x.fromUser.photo,
                    timestamp = x.timestamp,
                }).ToList();
                return list;
            
        }
        public EnRegisterEdit GetEditProfile(int id)
        {
            var result = db.users.Where(x => x.id == id).Select(data => new EnRegisterEdit
            {
                aboutMe = data.aboutMe,
                age = data.age,
                email = data.email,
                alcoholConsumption = data.alcoholConsumptionId.HasValue ? new EnDDL { id = data.alcoholConsumption.id, name = data.alcoholConsumption.name } : null,
                annualIncome = data.annualIncomeId.HasValue ? new EnDDL { id = data.annualIncome.id, name = data.annualIncome.name } : null,
                children = data.childrenId.HasValue ? new EnDDL { id = data.children.id, name = data.children.name } : null,
                fetiches = data.fetichesId.HasValue ? new EnDDL { id = data.fetiches.id, name = data.fetiches.name } : null,
                gender = data.genderId.HasValue ? new EnDDL { id = data.gender.id, name = data.gender.name } : null,
                height = data.height,
                lookingGender = data.lookingGenderId.HasValue ? new EnDDL { id = data.lookingGender.id, name = data.lookingGender.name } : null,
                myProfession = data.myProfessionId.HasValue ? new EnDDL { id = data.myProfession.id, name = data.myProfession.name } : null,
                name = data.name,
                profession = data.profession,
                physicalType = data.physicalTypeId.HasValue ? new EnDDL { id = data.physicalType.id, name = data.physicalType.name } : null,
                relationshipStatus = data.relationshipStatusId.HasValue ? new EnDDL { id = data.relationshipStatus.id, name = data.relationshipStatus.name } : null,
                password = data.password,
                zipcode = data.zipcode,
                whereamiknow = data.whereamiknow.HasValue ? new EnDDL { id = data.whereamiknow.Value, name = data.whereamiknow == 1 ? "Yes" : "No" } : null,
                vaccine = data.vaccineId.HasValue ? new EnDDL { id = data.vaccine.id, name = data.vaccine.name } : null,
                smoke = data.smokeId.HasValue ? new EnDDL { id = data.smoke.id, name = data.smoke.name } : null,
                username = data.username,
                sign = data.signId.HasValue ? new EnDDL { id = data.sign.id, name = data.sign.name } : null,
                religion = data.religionId.HasValue ? new EnDDL { id = data.religion.id, name = data.religion.name } : null,
                sexualOrientation = data.sexualOrientationId.HasValue ? new EnDDL { id = data.sexualOrientation.id, name = data.sexualOrientation.name } : null,
                photo = data.photo,
                eduction = data.eduction,
                hideage = data.hideage,
                last_relationship = data.last_relationship,
                gelocationBydistance = data.gelocationBydistance.HasValue ? new EnDDL { id = data.gelocationBydistance.Value, name = data.gelocationBydistance == 1 ? "Yes" : "No" } : null,
                lookingRelations = data.lookingRelations.Select(y => new EnDDL { id = y.relation.id, name = y.relation.name }).ToList(),
                personalities = data.personalities.Select(y => new EnDDL { id = y.personality.id, name = y.personality.name }).ToList(),
                hobbies = data.hobbies.Select(y => new EnDDL { id = y.hobby.id, name = y.hobby.name }).ToList(),
                qualities = data.qualities.Select(y => new EnDDL { id = y.quality.id, name = y.quality.name }).ToList(),
                city = data.city,
                rating=data.rating,
                likes=data.likes,
               
            }).FirstOrDefault();
            return result;
        }
        private long GetTime(DateTime dt)
        {
            DateTime utcNow = dt;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long ts = (long)((utcNow - epoch).TotalMilliseconds)/1000;
            return ts;
        }
        public List<EnStory> GetStories()
        {

            var list = db.stories.OrderByDescending(x => x.id).Include(x=>x.photos).Include(x=>x.user).ToList().Select(x => new EnStory
            {
                userId = x.userId,
                name = x.user.name,
                userPhoto=x.user.photo,
                timestamp= GetTime(x.timestamp),
                photos = x.photos.OrderByDescending(y => y.id).Select(y => new EnStoryPhoto { photo = y.photo, content = y.content }).ToList()
            }).ToList();
            
            return list;
        }

        public void AddReportUser(EnReport data)
        {
            var e = db.reportUsers.Where(x => x.fromUserId == data.fromUserId && x.toUserId == data.toUserId).FirstOrDefault();
            if (e != null)
            {
                e.reason = data.reason;
            }
            else
            {
                db.reportUsers.Add(new ReportUser { fromUserId = data.fromUserId, toUserId = data.toUserId, reason = data.reason, timestamp = DateTime.Now });
            }
            db.SaveChanges();
        }
    }
}
