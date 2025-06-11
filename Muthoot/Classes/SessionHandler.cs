using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Muthoot.Classes;
using Nancy;

namespace Muthoot.Classes
{
    [Serializable]
    public class SessionHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SessionHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ValidateSession()
        {
            var session = _contextAccessor.HttpContext.Session;

            if (string.IsNullOrEmpty(session.GetString("userId")))
            {
                _contextAccessor.HttpContext.Response.Redirect(_contextAccessor.HttpContext.Request.PathBase + "/Login/SessionExpired");
            }
        }

        public void ClearSessions()
        {
            var session = _contextAccessor.HttpContext.Session;

            session.Remove("userId");
            session.Remove("privilege");

            ValidateSession();
        }

        public void ValidateSession(ref UserInfo _UserDetails)
        {
            var session = _contextAccessor.HttpContext.Session;
            var json = session.GetString("UserDetails");

            if (string.IsNullOrEmpty(json))
            {
                _contextAccessor.HttpContext.Response.Redirect(_contextAccessor.HttpContext.Request.PathBase + "/Login/SessionExpired");
            }
            else
            {
                _UserDetails = System.Text.Json.JsonSerializer.Deserialize<UserInfo>(json);
            }
        }
    }
}
