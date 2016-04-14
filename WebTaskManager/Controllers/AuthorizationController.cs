using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebTaskManager.Manager;
using WebTaskManager.Models.repository;
using WebTaskManager.Repository;

namespace WebTaskManager.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly UserManager _userManager = new UserManager();

        [AllowAnonymous]
        public JsonResult Login(string login, string userPassword)
        {
            string setCoockie = String.Empty;
            var result = _userManager.UserLogin(login, userPassword, out setCoockie);
            if (setCoockie != String.Empty)
            {
                if (HttpContext.Response.Cookies["id"] == null)
                {
                    HttpContext.Response.Cookies.Add(new HttpCookie("id", setCoockie));
                }
                else
                {
                    HttpContext.Response.Cookies["id"].Value = setCoockie;
                }
            }

            return result;
        }
    }
}