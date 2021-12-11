using System.Net;
using System.Net.Mail;
// using Mailjet.Client;
// using Mailjet.Client.Resources;
// using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace miniproject.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            EmailOptions = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions EmailOptions { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(EmailOptions, subject, message, email);
        }

        public Task Execute(AuthMessageSenderOptions mailOption, string subject, string message, string email)
        {
            var client = new SmtpClient(mailOption.SMTPServer) {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    mailOption.ApiKey, 
                    mailOption.SecretKey
                ),
                EnableSsl = true,
                Port = mailOption.Port,

            };
            var mailMessage = new MailMessage {
                From = new MailAddress(
                    "info@studiolearning.online",
                    subject
                )
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            return client.SendMailAsync(mailMessage);
        }
    }
}