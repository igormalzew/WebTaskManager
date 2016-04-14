using System.Web;
using WebTaskManager.Manager;
namespace WebTaskManager.Filters
{
    public static class AccessVerify
    {
        static private readonly UserManager UserManager = new UserManager();

        public static string GetNameAuthorizedUser(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies["id"] == null) return null;

            var userCoockie = httpContext.Request.Cookies["id"].Value;
            var userInfo = UserManager.GetCoockieRecord(userCoockie);

            return userInfo?.User?.Name;
        }
    }
}