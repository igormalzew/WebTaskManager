using System;
using System.Web.Mvc;
using WebTaskManager.Filters;
using WebTaskManager.Models.repository;
using WebTaskManager.Repository;

namespace WebTaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserRepository _userRepository = new UserRepository();

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.IsAuthorized = false;
            return View();
        }

        [MyAuth]
        public ActionResult MainPage()
        {
            ViewBag.IsAuthorized = true;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Registration()
        {
            ViewBag.IsAuthorized = false;
            return View();
        }

        [MyAuth]
        public ActionResult LogOut()
        {
            return Redirect("/Home/Index");
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";


            var userCoockie = HttpContext.Request.Cookies["id"];
            if (userCoockie == null)
            {
                ViewBag.IsAuthorized = false;
            }
            else
            {
                var coockie = _userRepository.GetCoockieRecord(userCoockie.Value);
                if (coockie == null)
                {
                    ViewBag.IsAuthorized = false;
                }
                else
                {
                    ViewBag.Name = coockie.User.Name;
                    ViewBag.IsAuthorized = true;
                }
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            var userCoockie = HttpContext.Request.Cookies["id"];
            if (userCoockie == null)
            {
                ViewBag.IsAuthorized = false;
            }
            else
            {
                var coockie = _userRepository.GetCoockieRecord(userCoockie.Value);
                if (coockie == null)
                {
                    ViewBag.IsAuthorized = false;
                }
                else
                {
                    ViewBag.Name = coockie.User.Name;
                    ViewBag.IsAuthorized = true;
                }
            }

            return View();
        }

        [AllowAnonymous]
        public JsonResult AddNewUser(string birthDay, string email, string login, string name, string password)
        {
            var user = _userRepository.GetUserAuthorizationInfobyLogin(login);
            if (user == null)
            {
                return new JsonResult
                {
                    Data = new ErrorResult { IsError = true, ErrorMessage = "Указанный логин занят" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            try
            {
                DateTime outDate;
                DateTime.TryParse(birthDay, out outDate);
                _userRepository.AddNewUser(name, login, password, email, outDate);
                return new JsonResult
                {
                    Data = new ErrorResult { IsError = false, ErrorMessage = "Регистрация прошла успешно. Теперь вы можете <a href=\"~/Home/Index\">авторизоваться</a>" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception)
            {
                return new JsonResult
                {
                    Data = new ErrorResult { IsError = true, ErrorMessage = "Внутренняя ошибка сервера. Пожалуйста, повторите попытку." },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return null;
        }
    }
}