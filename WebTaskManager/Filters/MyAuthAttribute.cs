using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WebTaskManager.Repository;

namespace WebTaskManager.Filters
{
    public class MyAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        private readonly UserRepository _userRepository = new UserRepository();
        public void OnAuthentication(AuthenticationContext filterContext)
        {

            var userCoockie = filterContext.HttpContext.Request.Cookies["id"];
            if (userCoockie == null || _userRepository.GetCoockieRecord(userCoockie.Value) == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var userCoockie = filterContext.HttpContext.Request.Cookies["id"];
            if (userCoockie == null || _userRepository.GetCoockieRecord(userCoockie.Value) == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                    { "controller", "Home" }, { "action", "Index" }
                   });
            }
        }
    }
}