using System;
using System.Text;
using System.Web.Mvc;
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
                return new JsonResult
                {
                    Data = new ErrorResult { IsError = true, ErrorMessage = "Проверьте введенные данные" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            var loseCount = _userRepository.GetLoseAuthorizationCount(login);
            if (loseCount >= LOSE_COUNT) return new JsonResult
            {
                Data = new ErrorResult { IsError = true, ErrorMessage = "Доступ к учетной записи заблокирован на 1 час" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            var hashStr = Crypto.Crypto.GetHash(userPassword + userInfo.Salt);

            if (hashStr == userInfo.HashPassword)
            {
                try
                {
                    var saltByCoockie = Crypto.Crypto.GenerateRandomSalt();
                    var coockie = Crypto.Crypto.GetHash(hashStr + saltByCoockie);

                    _userRepository.AddCoockieRecord(login, coockie);

                    HttpContext.Response.Cookies["id"].Value = coockie;
                }
                catch (Exception)
                {
                    return new JsonResult
                    {
                        Data = new ErrorResult { IsError = true, ErrorMessage = "Ошибка авторизации. Попробуйте еще раз" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            else
            {
                _userRepository.AddAuthorizationLoseRecord(login);
                return new JsonResult
                {
                    Data = new ErrorResult
                    {
                        IsError = true,
                        ErrorMessage = LOSE_COUNT - loseCount - 1 > 0 ?
                    String.Format("Пароль введен неверно. Осталось {0} попытка", LOSE_COUNT - loseCount - 1) :
                    "Доступ к учетной записи заблокирован на 1 час"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return null;
        }
    }
}