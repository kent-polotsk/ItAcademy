using GNA.Services.Abstractions;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;


namespace GNA.Services.Implementations
{
    public class EmailService : IEmailService

    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;

        public EmailService( IConfiguration configuration)
        {
            _configuration = configuration;

            _smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]), //_emailSettings.Port,
                Credentials = new NetworkCredential(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]),
                EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]) //_emailSettings.EnableSsl
            };
        }

        public async Task SendEmailAsync(string toEmail, string code="")
        {
            const string SUBJECT = "АГРЕГАТОР ХОРОШИХ НОВОСТЕЙ - регистрация";
            string body = $"Данное письмо сгенерировано автоматически и не требует ответа.\n\n" +
                          $"Ваш код подтверждения : {code}\n\n" +
                          $"Никому не сообщайте ваш одноразовый код подтверждения. Если вы не запрашивали код то просто игнорируйте данное сообщение."; ;

            var mailMessage = new MailMessage(_configuration["EmailSettings:Username"], toEmail, SUBJECT, body);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}

