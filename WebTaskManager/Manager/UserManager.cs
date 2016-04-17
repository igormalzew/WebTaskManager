using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTaskManager.Models.repository;
using WebTaskManager.Repository;

namespace WebTaskManager.Manager
{
    public class UserManager
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public CoockieByLogin GetCoockieRecord(string coockie)
        {
            return _userRepository.GetCoockieRecord(coockie);
        }

        public JsonResult AddNewUser(string birthDay, string email, string login, string name, string password)
        {
            var user = _userRepository.GetUserAuthorizationInfobyLogin(login);
            if (user != null)
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
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new ErrorResult { IsError = true, ErrorMessage = "Внутренняя ошибка сервера. Пожалуйста, повторите попытку." },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }

        public bool UserLogOut(string userCoockie)
        {
            try
            {
                _userRepository.UserLogOut(userCoockie);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public JsonResult UserLogin(string login, string userPassword, out string setCoockie)
        {
            const int LOSE_COUNT = 5;
            setCoockie = String.Empty;

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

                    setCoockie = coockie;

                    return new JsonResult
                    {
                        Data = new ErrorResult { IsError = false, ErrorMessage = "Авторизация прошла успешно" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                catch (Exception ex)
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
                    String.Format("Пароль введен неверно. Осталось {0} попытк{1}", LOSE_COUNT - loseCount - 1, LOSE_COUNT - loseCount - 1 == 1 ? "а" : "и") :
                    "Доступ к учетной записи заблокирован на 1 час"
                    },JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

        }

        public JsonResult GetTasks(int userId, DateTime startDate, DateTime endDate, bool completeShow)
        {
            var tasks = _userRepository.GetTasks(userId ,startDate, endDate, Convert.ToInt32(completeShow));


            return new JsonResult
            {
                Data = new 
                {
                    tasks
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}