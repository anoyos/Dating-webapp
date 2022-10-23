using james.Helpers.Custom;
using james.Models;
using james.Models.DB;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Helpers.General
{
    public class SessionHelper
    {
        HttpRequest _request;
        HttpResponse _response;
        DBContext _db;
        public SessionHelper(DBContext db, HttpRequest request, HttpResponse response)
        {
            this._request = request;
            this._response = response;
            this._db = db;
        }


        public void set(string username)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            _response.Cookies.Append(Constants.LoginKey, Common.Encrypt(username, Constants.EncKey), option);
        }
        public tblLogin get()
        {
            var value = _request.Cookies[Constants.LoginKey];

            if (!string.IsNullOrEmpty(value))
            {

                var email = Common.Decrypt(value, Constants.EncKey);
                var Data = CommonModel.GetUserInfo(_db, email);
                return Data;
            }
            return null;
        }

        public void setCookieAccept(bool val)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            _response.Cookies.Append(Constants.CookieKey, Common.Serialize(val), option);
        }
        public bool? getCookieAccept()
        {
            var value = _request.Cookies[Constants.CookieKey];

            if (!string.IsNullOrEmpty(value))
            {
                return Common.Deserialize<bool>(value);
            }
            return null;
        }

        public void delete()
        {
            try
            {
                _response.Cookies.Delete(Constants.LoginKey);
            }
            catch { }
        }


    }
}
