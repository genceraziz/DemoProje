using System.Net;
using System.Net.Mail;

namespace InGameDemo.WebApi.Infrastructure
{
    public static class Tools
    {
        public static void SendEmail(string body, string userEmail, string userName)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.To.Add(new MailAddress(userEmail, userName));
                mailMessage.From = new MailAddress("ingamedemoproje@gmail.com", "InGame");
                mailMessage.Subject = "InGame - Şifre Sıfırlama Linki";
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;

                using (var mailClient = new SmtpClient("smtp.gmail.com"))
                {
                    mailClient.Port = 587;
                    mailClient.Credentials = new NetworkCredential("ingamedemoproje@gmail.com", "12346ingame");
                    mailClient.EnableSsl = true;

                    mailClient.SendMailAsync(mailMessage).Wait();
                }
            }
        }
    }
}
