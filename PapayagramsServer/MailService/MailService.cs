using MailKit.Net.Smtp;
using MimeKit;
using System.Configuration;

namespace MailService
{
    public class MailService
    {
        private static string _papayagramsAccount = ConfigurationManager.AppSettings["papayagramsEmail"];
        private static string _papayagramsPassword = ConfigurationManager.AppSettings["papayagramsPassword"];
        private static string _smtpServer = "smtp.gmail.com";
        private static int _smtpPort = 587;

        public static int SendMail(string receiverEmail, string subject, string body)
        {
            var mail = new MimeMessage();
            
            mail.From.Add(MailboxAddress.Parse(_papayagramsAccount));
            mail.To.Add(MailboxAddress.Parse(receiverEmail));

            mail.Subject = subject;
            mail.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = body
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_papayagramsAccount, _papayagramsPassword);
                smtpClient.Send(mail);
                smtpClient.Disconnect(true);
            }

            return 0;
        }
    }
}
