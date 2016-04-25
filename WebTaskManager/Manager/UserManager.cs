using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebTaskManager.Models.repository;
using WebTaskManager.Repository;

namespace WebTaskManager.Manager
{
    public class UserManager
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public CoockieByLogin GetCoockieRecord(string coockie)
        {
            try
            {
                return _userRepository.GetCoockieRecord(coockie);
            }
            catch (Exception ex)
            {
                return null;
            }
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

        public JsonResult GetTasks(int userId, string filter)
        {
            var inputFilter = JsonConvert.DeserializeObject<UserFilter>(filter);

            List<Task> tasks;
            if (inputFilter != null)
            {
                DateTime start;
                DateTime end;
                DateTime.TryParse(inputFilter.StartDate, out start);
                DateTime.TryParse(inputFilter.StartDate, out end);
                tasks = _userRepository.GetTasks(userId, inputFilter.PriorityFilter, inputFilter.CategoryFilter,
                    start, end, Convert.ToInt32(inputFilter.IsPerformanceFilter));
            }
            else
            {
                tasks = _userRepository.GetAllTasks(userId);
            }

            var result = tasks.Select(c => new
            {
                c.TaskId,
                c.TaskName,
                c.FullDescription,
                c.CategoryId,
                c.IsPerformance,
                c.PriorityId,
                SetDate = c.SetDate.ToString("dd.MM.yyyy"),
                c.SpendTime

            }).ToArray();

            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetCategory(int userId)
        {
            var categoryTypes = _userRepository.GetCategory(userId);

            var result = categoryTypes.Select(c => new
            {
                c.CategoryTypeId,
                c.CategoryName

            }).ToArray();

            return new JsonResult
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddCategory(int userId, string category)
        {
            var inputCategory = JsonConvert.DeserializeObject<string[]>(category);
            foreach (var item in inputCategory)
            {
                _userRepository.AddCategory(userId, item);
            }

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveCategory(int userId, string category)
        {
            var inputCategoryIds = JsonConvert.DeserializeObject<int[]>(category);
            foreach (var id in inputCategoryIds)
            {
                _userRepository.RemoveCategory(userId, id);
            }

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}