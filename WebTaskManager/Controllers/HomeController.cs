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
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return View();
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userInfo[0];
            return RedirectToAction("MainPage", "home");
        }


        public ActionResult MainPage()
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return RedirectToAction("index", "home");
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userInfo[0];
            return View();
        }

        public ActionResult Registration()
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return View();
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userInfo[0];
            return View();
        }

        public ActionResult LogOut()
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo == null)
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
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return View();
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userInfo[0];
            return View();
        }

        public ActionResult Contact()
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo == null)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                return View();
            }

            ViewBag.IsAuthorized = true;
            ViewBag.UserName = userInfo[0];
            return View();
        }

        public JsonResult AddNewUser(string birthDay, string email, string login, string name, string password)
        {
            return _userManager.AddNewUser(birthDay, email, login, name, password);
        }

        public JsonResult GetTasks(string filter)
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo != null)
            {
                return _userManager.GetTasks(Convert.ToInt32(userInfo[1]), filter);
            }
            return null;
        }

        public JsonResult GetCategory()
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo != null)
            {
                return _userManager.GetCategory(Convert.ToInt32(userInfo[1]));
            }
            return null;
        }

        public JsonResult AddCategory(string category)
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo != null)
            {
                return _userManager.AddCategory(Convert.ToInt32(userInfo[1]), category);
            }
            return null;
        }

        public JsonResult RemoveCategory(string category)
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo != null)
            {
                return _userManager.RemoveCategory(Convert.ToInt32(userInfo[1]), category);
            }
            return null;
        }

        public JsonResult AddNewTask(string taskData)
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo != null)
            {
                return _userManager.AddNewTask(Convert.ToInt32(userInfo[1]), taskData);
            }
            return null;
        }

        public JsonResult SaveTask(string taskData)
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo != null)
            {
                return _userManager.SaveTask(Convert.ToInt32(userInfo[1]), taskData);
            }
            return null;
        }

        public JsonResult RemoveTask(int taskId)
        {
            var userInfo = AccessVerify.GetInfoAuthorizedUser(System.Web.HttpContext.Current);
            if (userInfo != null)
            {
                return _userManager.RemoveTask(Convert.ToInt32(userInfo[1]), taskId);
            }
            return null;
        }

        public ActionResult PassRecovery()
        {
            ViewBag.IsAuthorized = false;
            ViewBag.UserName = String.Empty;
            return View();
        }

        public JsonResult PassRecoveryRequest(string email)
        {
            return _userManager.PassRecoveryRequest(email);
        }

        public ActionResult EmailPassRecovery(int userId, string hash)
        {
            var coockieByLogin = _userManager.GetCoockieRecord(hash);
            if (coockieByLogin != null && coockieByLogin.UserId == userId)
            {
                ViewBag.IsAuthorized = false;
                ViewBag.UserName = String.Empty;
                ViewBag.userHash = hash;
                return View();
            }
            ViewBag.IsAuthorized = false;
            ViewBag.UserName = String.Empty;
            return RedirectToAction("index", "home");
        }
        public JsonResult SaveRecoveryPass(string hash, string pass)
        {
            return _userManager.SaveRecoveryPass(hash, pass);
        }
        
    }
}