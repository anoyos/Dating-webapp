using james.Helpers.Custom;
using james.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Models
{
    public class CommonModel
    {
        public static tblLogin GetUserInfo(DBContext db, string username)
        {
            tblLogin login = null;

            var a = db.users.Where(x => !x.isdeleted && (x.username == username)).FirstOrDefault();
            if (a != null)
            {
                //return new tblLogin { name = a.name, username = email, id = a.id, roleId = a.roleId, photo = a.photo, companyId = a.companies.Select(x => x.companyId).FirstOrDefault() };
                return new tblLogin { name = a.name, username = a.username, id = a.id, roleId = a.roleId, photo = a.photo };
            }
            return login;
        }
        public static string GetTimeAgoFormat(DateTime yourDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.Now.Ticks - yourDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }

        }
    }
}
