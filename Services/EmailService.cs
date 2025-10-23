using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using ContactProMVC.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ContactProMVC.Services
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var sender = _mailSettings.Email ?? Environment.GetEnvironmentVariable("Email");
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(sender),
                Subject = subject
            };

            foreach (var emailAddress in email.Split(";"))
            {
                mail.To.Add(MailboxAddress.Parse(emailAddress));
            }

            var emailBody = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            mail.Body = emailBody.ToMessageBody();

            using SmtpClient smtp = new();

            try
            {
                var host = _mailSettings.Host ?? Environment.GetEnvironmentVariable("Host");
                var port = _mailSettings.Port != 0 ? _mailSettings.Port : int.Parse(Environment.GetEnvironmentVariable("Port")!);
                var password = _mailSettings.Password ?? Environment.GetEnvironmentVariable("Password");

                await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(sender, password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                throw;
            }
        }
    }
}