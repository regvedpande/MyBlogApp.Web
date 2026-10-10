using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace MyBlogApp.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var smtp = _config.GetSection("Smtp");
            var host = smtp.GetValue<string>("Host");
            var port = smtp.GetValue<int>("Port");
            var enableSsl = smtp.GetValue<bool>("EnableSsl");
            var user = smtp.GetValue<string>("User");
            var pass = smtp.GetValue<string>("Pass");
            var from = smtp.GetValue<string>("From");
            var display = smtp.GetValue<string>("DisplayName");

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(from))
            {
                return;
            }

            var message = new MailMessage();
            message.From = new MailAddress(from, display);
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = htmlBody;
            message.IsBodyHtml = true;

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = enableSsl;
                if (!string.IsNullOrWhiteSpace(user))
                {
                    client.Credentials = new NetworkCredential(user, pass);
                }

                // SmtpClient doesn't have SendMailAsync in all target frameworks; wrap in Task.Run for compatibility
                await Task.Run(() => client.Send(message));
            }
        }
    }
}
