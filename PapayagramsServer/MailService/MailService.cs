using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace MailService
{
    public static class MailService
    {
        private static readonly string _papayagramsAccount = "papayagrams@gmail.com";
        private static readonly string _papayagramsPassword = Environment.GetEnvironmentVariable("Papayagrams_EmailPassword");
        private static readonly string _smtpServer = "smtp.gmail.com";
        private static readonly int _smtpPort = 587;

        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="receiverEmail">destination email</param>
        /// <param name="subject">subject of the email</param>
        /// <param name="body">message of the email</param>
        /// <returns>0 if the email was sent correctly, an exception otherwise</returns>
        /// <exception cref="SmtpCommandException">Thrown when the process fails</exception>"
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
