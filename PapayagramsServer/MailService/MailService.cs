using MailKit.Net.Smtp;
using MimeKit;
using System.Configuration;
using System.Threading.Tasks;

namespace MailService
{
    public class MailService
    {
        private string _papayagramsAccount;
        private string _papayagramsPassword;
        private const string _smtpServer = "smtp.gmail.com";
        private const int _smtpPort = 587;

        public MailService()
        {
            _papayagramsAccount = ConfigurationManager.AppSettings["papayagramsEmail"];
            _papayagramsPassword = ConfigurationManager.AppSettings["papayagramsPassword"];
        }

        public Task SendMail(string receiverEmail, string subject, string body)
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

            return Task.CompletedTask;
        }
    }
}
