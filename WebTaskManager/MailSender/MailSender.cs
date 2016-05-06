using System;
using System.Net.Mail;
using System.Net;

namespace WebTaskManager.MailSender
{
    public class MailSender
    {
        public static void SendMessage(string userEmail, string userMessage)
        {
            SmtpClient Smtp = new SmtpClient("smtp.mail.ru", 2525);
            Smtp.Credentials = new NetworkCredential("webtaskmanager@mail.ru", "n?*n5x)j7TlZN?B18hkSH<3aR*>Q;>");
            Smtp.EnableSsl = true;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("webtaskmanager@mail.ru");
            message.To.Add(new MailAddress(userEmail));
            message.Subject = "Восстановление пароля";
            message.IsBodyHtml = true;

            var link = String.Format("Ссылка для восстановления пароля: <a href={0}>задать новый пароль</a>", userMessage);
            message.Body = String.Format(@"<html>
                                               <p>{0}</p>
                                        </html>", link);
   
            Smtp.Send(message);

        }
    }
}