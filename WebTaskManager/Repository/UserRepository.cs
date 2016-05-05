using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebTaskManager.Models.repository;

namespace WebTaskManager.Repository
{
    public class UserRepository
    {
        private readonly DBTaskManagerEntities _model = new DBTaskManagerEntities();
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
                where t.UserId == userId && t.SetDate >= startDate &&
                      t.SetDate <= endDate && (isPerformanceFilter == 1 || t.IsPerformance != 1) &&
                      priorityFilter.Contains(t.PriorityId) &&
                      (isCaregoryArrEmpty || t.Category.Where(c => c.TaskId == t.TaskId).Any(c => categoryFilter.Contains(c.CategoryTypeId)))
                        select t;

            return tasks.ToList();
        }

        public List<Task> GetAllTasks(int userId)
        {
            var todayFull = DateTime.Now;
            var todayStr = todayFull.ToShortDateString();
            DateTime today;
            DateTime.TryParse(todayStr, out today);

            var tasks = from t in _model.Task
                        where t.UserId == userId && t.SetDate >= today && t.SetDate <= today && t.IsPerformance == 0 // Задача не выполнена
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

        public void AddNewTask(int userId, TaskData taskData)
        {
            DateTime setDate;
            DateTime.TryParse(taskData.TaskDate, out setDate);

            var task = new Task
            {
                UserId = userId,
                TaskName = taskData.Name,
                FullDescription = taskData.Descriptyon,
                PriorityId = taskData.Priority,
                SetDate = setDate,
                IsPerformance = Convert.ToInt32(taskData.IsPerformance),
                SpendTime = taskData.SpendTime
            };
            _model.Task.Add(task);

            if (taskData.TaskCategory != null)
            {
                foreach (var categoryType in taskData.TaskCategory)
                {
                    var cat = new Category
                    {
                        Task = task,
                        CategoryTypeId = categoryType
                    };
                    _model.Category.Add(cat);
                }
            }
            _model.SaveChanges();
        }

        public void SaveTask(int userId, TaskData taskData)
        {
            DateTime setDate;
            DateTime.TryParse(taskData.TaskDate, out setDate);

            var task = _model.Task.FirstOrDefault(c => c.UserId == userId && c.TaskId == taskData.TaskId);

            if(task == null) return;

            task.TaskName = taskData.Name;
            task.FullDescription = taskData.Descriptyon;
            task.PriorityId = taskData.Priority;
            task.SetDate = setDate;
            task.IsPerformance = Convert.ToInt32(taskData.IsPerformance);
            task.SpendTime = taskData.SpendTime;

            _model.Category.RemoveRange(_model.Category.Where(c => c.TaskId == task.TaskId));

            if (taskData.TaskCategory != null)
            {
                foreach (var categoryType in taskData.TaskCategory)
                {
                    var cat = new Category
                    {
                        TaskId = task.TaskId,
                        CategoryTypeId = categoryType
                    };
                    task.Category.Add(cat);
                }
            }

            _model.SaveChanges();
        }

        public void RemoveTask(int userId, int taskId)
        {
            var task = _model.Task.FirstOrDefault(t => t.UserId == userId && t.TaskId == taskId);

            if (task == null) return;

            _model.Task.Remove(task);
            _model.SaveChanges();
        }
    }
}