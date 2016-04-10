using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebTaskManager.Filters;
using WebTaskManager.Models.repository;
using WebTaskManager.Repository;

namespace WebTaskManager.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly UserRepository _userRepository = new UserRepository();

        [AllowAnonymous]
        public JsonResult Login(string login, string userPassword)
        {
            const int LOSE_COUNT = 5;

            var userInfo = _userRepository.GetUserAuthorizationInfobyLogin(login);
            if (userInfo == null)
            {
                return new JsonResult { Data = new ErrorResult { IsError = true, ErrorMessage = "Проверьте введенные данные"},
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            }
            var loseCount = _userRepository.GetLoseAuthorizationCount(login);
            if(loseCount >= LOSE_COUNT) return new JsonResult { Data = new ErrorResult { IsError = true, ErrorMessage = "Доступ к учетной записи заблокирован на 1 час" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet };


            var hmacsha1 = new HMACSHA1();
            byte[] bytes = Encoding.Default.GetBytes(userPassword + userInfo.Salt);
            var hashBytes = hmacsha1.ComputeHash(bytes);
            string hashStr = Encoding.Default.GetString(hashBytes);

            if (hashStr == userInfo.HashPassword)
            {
                try
                {
                    byte[] bytesCoockie = Encoding.Default.GetBytes(hashStr);
                    var hashBytesCoockie = hmacsha1.ComputeHash(bytesCoockie);
                    string hashStrCoockie = Encoding.Default.GetString(hashBytesCoockie);
                    _userRepository.AddCoockieRecord(login ,hashStrCoockie);

                    HttpContext.Response.Cookies["id"].Value = hashStrCoockie;
                }
                catch (Exception)
                {
                    return new JsonResult { Data = new ErrorResult { IsError = true, ErrorMessage = "Ошибка авторизации. Попробуйте еще раз" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            else
            {
                _userRepository.AddAuthorizationLoseRecord(login);
                return new JsonResult { Data = new ErrorResult { IsError = true, ErrorMessage = LOSE_COUNT - loseCount - 1 > 0 ?
                    String.Format("Пароль введен неверно. Осталось {0} попыток", LOSE_COUNT - loseCount - 1) :
                    "Доступ к учетной записи заблокирован на 1 час"
                }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return null;
        }
    }
}