using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models.DB
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }  
        public string photo { get; set; }
        public string banner { get; set; }
        public int roleId { get; set; }
        public bool isApprove{ get; set; }
        public bool isBlocked { get; set; }
        public bool isFlagged { get; set; }



        public int? age { get; set; }

        [ForeignKey("gender")]
        public int? genderId { get; set; }
        public virtual DDL gender { get; set; }
        

        public ICollection<UserLookingRelation> lookingRelations { get; set; }


        [ForeignKey("lookingGender")]
        public int? lookingGenderId { get; set; }
        public virtual DDL lookingGender { get; set; }


        public string eduction { get; set; }
        public string profession { get; set; }

        [ForeignKey("annualIncome")]
        public int? annualIncomeId { get; set; }
        public virtual DDL annualIncome { get; set; }

        public string aboutMe { get; set; }
        public string last_relationship { get; set; }

        [ForeignKey("alcoholConsumption")]
        public int? alcoholConsumptionId { get; set; }
        public virtual DDL alcoholConsumption { get; set; }


        [ForeignKey("smoke")]
        public int? smokeId { get; set; }
        public virtual DDL smoke { get; set; }


        [ForeignKey("fetiches")]
        public int? fetichesId { get; set; }
        public virtual DDL fetiches { get; set; }


        [ForeignKey("sexualOrientation")]
        public int? sexualOrientationId { get; set; }
        public virtual DDL sexualOrientation { get; set; }

        [ForeignKey("relationshipStatus")]
        public int? relationshipStatusId { get; set; }
        public virtual DDL relationshipStatus { get; set; }


        [ForeignKey("vaccine")]
        public int? vaccineId { get; set; }
        public virtual DDL vaccine { get; set; }

        [ForeignKey("children")]
        public int? childrenId { get; set; }
        public virtual DDL children { get; set; }



        public ICollection<UserPersonality> personalities { get; set; }
        public ICollection<UserQuality> qualities { get; set; }

        [ForeignKey("myProfession")]
        public int? myProfessionId { get; set; }
        public virtual DDL myProfession { get; set; }


        public string height { get; set; }

        [ForeignKey("physicalType")]
        public int? physicalTypeId { get; set; }
        public virtual DDL physicalType { get; set; }


        [ForeignKey("religion")]
        public int? religionId { get; set; }
        public virtual DDL religion { get; set; }



        public ICollection<UserHobby> hobbies { get; set; }

        [ForeignKey("sign")]
        public int? signId { get; set; }
        public virtual DDL sign { get; set; }

        public string zipcode { get; set; }
        public bool hideage { get; set; }
        public int? gelocationBydistance { get; set; }
        public int? whereamiknow { get; set; }
        public bool isdeleted { get; set; }
        public int source { get; set; }
        public string token { get; set; }

        public string lat { get; set; }
        public string lng { get; set; }
        public string city { get; set; }
        public DateTime? lastLocTimestamp { get; set; }
        public double rating { get; set; }
        public int likes { get; set; }
        public int views { get; set; }
        [InverseProperty(nameof(UserRating.toUser))]
        public ICollection<UserRating> ratings { get; set; }
        public ICollection<Diary> diaries { get; set; }
        public ICollection<Album> albums { get; set; }
    }
    public class DDL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }

        public int type { get; set; }
        public bool isDeleted { get; set; }
    }
    public class UserLookingRelation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("relation")]
        public int relationId { get; set; }
        public virtual DDL relation { get; set; }


        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }

    }
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("fromUser")]
        public int fromUserId { get; set; }
        public virtual User fromUser { get; set; }


        [ForeignKey("toUser")]
        public int toUserId { get; set; }
        public virtual User toUser { get; set; }

        public int type { get; set; }
        public DateTime timestamp { get; set; }

    }

    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string location { get; set; }
        public string photo { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string description { get; set; }
        public DateTime timestamp { get; set; }
        public double amount { get; set; }
        public bool isPublish { get; set; }
        public bool isDeleted { get; set; }
    }

    public class AppSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string appVersion { get; set; }
        public string androidVersion { get; set; }
        public string iosAppId { get; set; }
        public string appSupportEmail { get; set; }
        public string privacyPolicyUrl { get; set; }
        public string firebaseServerKey { get; set; }
        public string firebaseSenderId { get; set; }
        public string freeMaxRadius { get; set; }
        public string vipMaxRadius { get; set; }
        public bool hiddenPassword { get; set; }
        public int hiddentPasswordCnt { get; set; }
        public bool userAutoApprove { get; set; }
    }

    public class UserPersonality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("personality")]
        public int personalityId { get; set; }
        public virtual DDL personality { get; set; }


        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }

    }


    public class UserQuality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("quality")]
        public int qualityId { get; set; }
        public virtual DDL quality { get; set; }

        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }

    }

    public class UserHobby
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("hobby")]
        public int hobbyId { get; set; }
        public virtual DDL hobby { get; set; }

        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }
    }
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string name { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }
        public DateTime timestamp { get; set; }
        public bool isPrivate { get; set; }
        public bool isDeleted { get; set; }
        public ICollection<AlbumImage> images { get; set; }
        public ICollection<HiddenAlbum> access { get; set; }
    }
    public class AlbumImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string photo { get; set; }
        [ForeignKey("album")]
        public int albumId { get; set; }
        public virtual Album album { get; set; }
        public DateTime timestamp { get; set; }
        public bool isDeleted { get; set; }
    }
    public class HiddenAlbum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public int status { get; set; }
        public DateTime timestamp { get; set; }
        [ForeignKey("album")]
        public int albumId { get; set; }
        public virtual Album album { get; set; }
        [ForeignKey("user")]
        public int? userId { get; set; }
        public virtual User user { get; set; }

    }
    public class UserRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string review { get; set; }
        public double rating { get; set; }
        public DateTime timestamp { get; set; }

        [ForeignKey("fromUser")]
        public int fromUserId { get; set; }
        public virtual User fromUser { get; set; }

        [ForeignKey("toUser")]
        public int toUserId { get; set; }
        public virtual User toUser { get; set; }
    }
    public class Diary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string photo { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }
    }
    
  
    public class ReportUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string reason { get; set; }
        public DateTime timestamp { get; set; }
        public int status { get; set; }

        [ForeignKey("fromUser")]
        public int fromUserId { get; set; }
        public virtual User fromUser { get; set; }

        [ForeignKey("toUser")]
        public int toUserId { get; set; }
        public virtual User toUser { get; set; }

    }
    public class VerificationCode
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string code { get; set; }
        public DateTime timestamp { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }
        public string uid { get;  set; }
    }
    public class FirebaseToken
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
     

        public string token { get; set; }
        public string device { get; set; }
        public bool isNotificationFlag { get; set; }
        public System.DateTime timestamp { get; set; }
        public string os { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }

    }
    public class ChatThread
    {
       
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int id { get; set; }

            [ForeignKey("user1")]
            public int user1Id { get; set; }
            public virtual User user1 { get; set; }

            [ForeignKey("user2")]
            public int user2Id { get; set; }
            public virtual User user2 { get; set; }

        [ForeignKey("chat")]
        public int? chatId { get; set; }
        public virtual Chat chat { get; set; }


        public string last_message { get; set; }
            public Nullable<System.DateTime> last_message_timestamp { get; set; }
            public int user1_unread { get; set; }
            public int user2_unread { get; set; }
      
            public int message_type { get; set; }
            public string attachment { get; set; }
            public string additional_info { get; set; }
            

          
        }
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int messageType { get; set; }
        public string message { get; set; }
        public string attachment { get; set; }
        public string duration { get; set; }
        public DateTime timestamp { get; set; }

        [ForeignKey("sender")]
        public int senderId { get; set; }
        public virtual User sender { get; set; }

        public bool isSeen { get; set; }
        [ForeignKey("chatThread")]
        public int chatThreadId { get; set; }
        public virtual ChatThread chatThread { get; set; }
    }

    public class CallLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public DateTime timestamp { get; set; }
        public string token { get; set; }
        public string channel { get; set; }
        public bool isVideo { get; set; }
        [ForeignKey("fromUser")]
        public int fromUserId { get; set; }
        public virtual User fromUser { get; set; }

        [ForeignKey("toUser")]
        public int toUserId { get; set; }
        public virtual User toUser { get; set; }
    }


    public class DiscountCoupon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public bool isActive { get; set; }
        public double percentage { get; set; }
        public bool isDeleted { get; set; }
    }

    public class Story
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime timestamp { get; set; }


        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }
        public bool isDeleted { get; set; }
        public ICollection<StoryPhoto> photos { get; set; }
    }
    public class StoryPhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string photo { get; set; }
        public string content { get; set; }
        [ForeignKey("story")]
        public int storyId { get; set; }
        public virtual Story story { get; set; }
    }



    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public string banner { get; set; }
        public string description { get; set; }
        public DateTime timestamp { get; set; }
        public bool isPublish { get; set; }
        public bool isDeleted { get; set; }
    }

    public class Streaming
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public string banner { get; set; }
        public string category { get; set; }
        public string token { get; set; }
        public string channel { get; set; }


        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime? startedOn { get; set; }
        public DateTime? endedOn { get; set; }
        public int status { get; set; }
        public ICollection<StreamingMember> members { get; set; }
        public ICollection<StreamingActivity> activities { get; set; }

    }
    public class StreamingMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }


        [ForeignKey("stream")]
        public int streamId { get; set; }
        public virtual Streaming stream { get; set; }

        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }
        public DateTime timestamp { get; set; }
      
    }
    public class StreamingActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int type { get; set; }
        public string detail { get; set; }
        [ForeignKey("stream")]
        public int streamId { get; set; }
        public virtual Streaming stream { get; set; }

        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual User user { get; set; }


        [ForeignKey("parent")]
        public int? parentId { get; set; }
        public virtual StreamingActivity parent { get; set; }


        public DateTime timestamp { get; set; }
    }
    public class ErrorLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string device { get; set; }
        public string detail { get; set; }
        public DateTime timestamp { get; set; }
    }
}
