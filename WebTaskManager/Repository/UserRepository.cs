using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using WebTaskManager.Models.repository;

namespace WebTaskManager.Repository
{
    public class UserRepository
    {
        private readonly DBTaskManagerContext _model = new DBTaskManagerContext();

        public AuthorizationUserInfo GetUserAuthorizationInfobyLogin(string login)
        {
            var user = _model.User.FirstOrDefault(c => c.Login == login);
            if (user == null) return null;

            return new AuthorizationUserInfo
            {
                Login = user.Login,
                Salt = user.Salt,
                HashPassword = user.HashPassword
            };
        }

        public int GetLoseAuthorizationCount(string login)
        {
            var user = _model.User.FirstOrDefault(c => c.Login == login);
            DateTime date = DateTime.Now.AddHours(-1);
            return _model.LoseAuth.Count(c => c.UserId == user.UserId && c.LoseTime > date);
        }

        public CoockieByLogin GetCoockieRecord(string coockie)
        {
            return _model.CoockieByLogin.FirstOrDefault(c => c.Coockie == coockie);
        }

        public void AddCoockieRecord(string login, string coockie)
        {
            var user = _model.User.FirstOrDefault(c => c.Login == login);
            if (user == null) return;

            var record = new CoockieByLogin
            {
                User = user,
                Coockie = coockie
            };
            _model.CoockieByLogin.Add(record);
            _model.SaveChanges();
        }


        public void AddAuthorizationLoseRecord(string login)
        {
            var user = _model.User.FirstOrDefault(c => c.Login == login);
            var loseRecord = new LoseAuth
            {
                User = user,
                LoseTime = DateTime.Now
            };

            _model.LoseAuth.Add(loseRecord);
            _model.SaveChanges();
        }

        public void AddNewUser(string name, string login, string password, string email, DateTime birthDay)
        {
            var userSalt = Crypto.Crypto.GenerateRandomSalt();
            var userHashPassword = Crypto.Crypto.GetHash(password + userSalt);
            var newUser = new User()
            {
                Name = name,
                Login = login,
                Salt = userSalt,
                HashPassword = userHashPassword,
                BirthDate = birthDay,
                Email = email
            };

            _model.User.Add(newUser);
            _model.SaveChanges();
        }

        public void UserLogOut(string userCoockie)
        {
            var coockieDb = _model.CoockieByLogin.FirstOrDefault(c => c.Coockie == userCoockie);
            if (coockieDb == null) return;

            _model.CoockieByLogin.Remove(coockieDb);
            _model.SaveChanges();
        }

        public List<Task> GetTasks(int userId, int[] priorityFilter, int[] categoryFilter, DateTime startDate, DateTime endDate, int isPerformanceFilter)
        {
            var isCaregoryArrEmpty = categoryFilter.Length == 0;

            var tasks = from t in _model.Task
                join c in _model.Category on t.TaskId equals c.TaskId
                where t.UserId == userId && t.SetDate >= startDate &&
                      t.SetDate <= endDate && (isPerformanceFilter == 1 || t.IsPerformance != isPerformanceFilter) &&
                      priorityFilter.Contains(t.PriorityId) && ((isCaregoryArrEmpty && t.CategoryId == null) ||
                                                                    (t.CategoryId != null && categoryFilter.Contains(c.CategoryId)))
                        select t;

            return tasks.ToList();
        }

        public List<Task> GetAllTasks(int userId)
        {
            var today = DateTime.Now;

            var tasks = from t in _model.Task
                        join c in _model.Category on t.TaskId equals c.TaskId
                        where t.UserId == userId && t.SetDate >= today &&
                              t.SetDate <= today
                        select t;

            return tasks.ToList();
        }

        public IQueryable<CategoryType> GetCategory(int userId)
        {
            var categoryTypes = from c in _model.CategoryType
                        where c.UserId == userId
                        select c;

            return categoryTypes;
        }

        public void AddCategory(int userId, string nameCategory)
        {
            var category = new CategoryType()
            {
                CategoryName = nameCategory,
                UserId = userId
            };

            _model.CategoryType.Add(category);
            _model.SaveChanges();
        }

        public void RemoveCategory(int userId, int categoryId)
        {
            var category = _model.CategoryType.FirstOrDefault(c => c.UserId == userId && c.CategoryTypeId == categoryId);
            if(category == null) return;
            
            _model.CategoryType.Remove(category);
            _model.SaveChanges();
        }
    }
}