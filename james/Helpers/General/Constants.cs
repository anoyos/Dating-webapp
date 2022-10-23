using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.General
{
    public class Constants
    {
        public static string EncKey = "AaECAwQFBgcICQoLDA0ODw==";
        public static String LoginKey = "JamesLogin";
        public static String CookieKey = "CookieAccept";
        public static string DateFormat = "yyyy-MM-dd";
        public static string socialPassword = "J@mEs007!";
    }
    public enum ResultStatus
    {
        Unauthorized = 0,
        Success = 1,
        Error = 2,
        NotFound = 3,
        Warning = 4,
        InProcess = 5
    }
    public enum RoleEnum
    {
        Admin=1,
        User=2
    }
    public enum DDLTypeEnum
    {
        Gender = 1,
        LookingRelation = 2,
        LookingGender = 3,
        AnnualIncome = 4,
        Alcohol = 5,
        Smokes = 6,
        SexualOrientation = 7,
        RelationshipStatus = 8,
        Vacine = 9,
        Children = 10,
        Personlity = 11,
        Qualities = 12,
        Myproffession = 13,
        PhysicalType = 14,
        Religon = 15,
        Hobbies = 16,
        Sign = 17,
        Fetches=18,
    }
    public enum UserSourceTypeEnum
    {
        Internal=1,
        Facebook=2,
        Google=3,
    }
    public enum MatchTypeEnum
    {
        Match=1,
        Like=2,
        SuperLike=3,
        Dislike
    }
    public enum HiddenAlbumCodeStatus
    {
        NotUsed = 1,
        Used = 2,    
    }
    public enum NotificationTypeEnum
    {
        NewMessage = 1,
        MatchFound=2,
        ReceiveCall=3,
        RingingCall=4,
        Rejected=5,
        UserBusy=6,
        Accepted=7,
        CallEnded=8,
    }
    public enum StreamStatus
    {
        NotStarted=1,
        Started=2,
        Ended=3,
    }
    public enum StreamingActivityEnum
    {
       Comment=1,
       View=2,
       Like=2,
    }
}
