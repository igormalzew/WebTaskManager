using System;
using System.Linq;
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
    }
}