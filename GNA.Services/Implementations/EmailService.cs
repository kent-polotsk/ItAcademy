using GNA.Services.Abstractions;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using DataConvert.Models;
using Microsoft.Extensions.Options;


namespace GNA.Services.Implementations
{
    public class EmailService : IEmailService

    {
        private readonly SmtpClient _smtpClient;
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;

            _smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSsl
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject = "", string body = "")
        {
            var mailMessage = new MailMessage(_emailSettings.Username, toEmail, subject, body);
            //_smtpClient.SendCompleted += (s, e) =>
            //{
            //    if (e.Error != null)
            //    {
            //        Console.WriteLine($"Ошибка отправки письма: {e.Error.Message}");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Письмо отправлено успешно.");
            //    }
            //};


            await _smtpClient.SendMailAsync(mailMessage);
        }
    }

}

