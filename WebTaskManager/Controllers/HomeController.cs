using System;
using System.Web.Mvc;
using WebTaskManager.Filters;
using WebTaskManager.Manager;

namespace WebTaskManager.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
       
       private readonly UserManager _userManager = new UserManager();

        public ActionResult Index()
        {
            var userName = AccessVerify.GetNameAuthorizedUser(System.Web.HttpContext.Current);
            if (userName == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return View();
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userName;
            return RedirectToAction("MainPage", "home");
        }


        public ActionResult MainPage()
        {
            var userName = AccessVerify.GetNameAuthorizedUser(System.Web.HttpContext.Current);
            if (userName == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return RedirectToAction("index", "home");
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userName;
            return View();
        }

        public ActionResult Registration()
        {
            var userName = AccessVerify.GetNameAuthorizedUser(System.Web.HttpContext.Current);
            if (userName == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return View();
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userName;
            return View();
        }

        public ActionResult LogOut()
        {
            var userName = AccessVerify.GetNameAuthorizedUser(System.Web.HttpContext.Current);
            if (userName == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return RedirectToAction("index", "home");
            }

            _userManager.UserLogOut(System.Web.HttpContext.Current.Request.Cookies["id"].Value);

            ViewBag.IsAuthorized = false;
            ViewBag.UserName = String.Empty;
            return RedirectToAction("index", "home");
        }

        public ActionResult About()
        {
            var userName = AccessVerify.GetNameAuthorizedUser(System.Web.HttpContext.Current);
            if (userName == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return View();
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userName;
            return View();
        }

        public ActionResult Contact()
        {
            var userName = AccessVerify.GetNameAuthorizedUser(System.Web.HttpContext.Current);
            if (userName == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return View();
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userName;
            return View();
        }

        public JsonResult AddNewUser(string birthDay, string email, string login, string name, string password)
        {
            return _userManager.AddNewUser(birthDay, email, login, name, password);
        }

        public JsonResult GetTasks()
        {
            var userName = AccessVerify.GetNameAuthorizedUser(System.Web.HttpContext.Current);
            if (userName != null)
            {
                var userId = AccessVerify.GetUserIdAuthorizedUser(System.Web.HttpContext.Current);
                if (userId == null) return null;


                return _userManager.GetTasks(Convert.ToInt32(userId), DateTime.Now, DateTime.Now, true);
            }
            return null;
        }
    }
}