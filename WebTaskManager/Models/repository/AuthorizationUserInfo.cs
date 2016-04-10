namespace WebTaskManager.Models.repository
{
    public class AuthorizationUserInfo
    {
        public string Login { get; set; }
        public string Salt { get; set; }
        public string HashPassword { get; set; }
    }

}