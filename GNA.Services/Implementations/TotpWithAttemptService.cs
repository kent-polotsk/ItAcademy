using System.Security.Cryptography;
using System.Text;
using GNA.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using OtpNet;

namespace GNA.Services.Implementations
{
    public class TotpWithAttemptService : ITotpWithAttemptService
    {
        private readonly IConfiguration _configuration;

        public TotpWithAttemptService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateTotpCode(string email)
        {
            var secretBytes = Encoding.UTF8.GetBytes(email+ _configuration["Security:SecretKey1"]);
            var totp = new Totp(secretBytes);
            return totp.ComputeTotp(); // Генерируем 6-значный код
        }

        
        public string GenerateToken(int attemptCount, DateTime lastAttempt)
        {
            var payload = $"{attemptCount}:{lastAttempt.ToUniversalTime():o}";
            var signature = ComputeHmac(payload, _configuration["Security:SecretKey2"]); // TokenSecret)// Подписываем
            return $"{payload}:{signature}";
        }

          
        public bool ValidateToken(string token, out int attemptCount, out DateTime lastAttempt)
        {
            attemptCount = 0;
            lastAttempt = DateTime.MinValue;

            var parts = token.Split(':');
            if (parts.Length != 3) return false;

            var payload = $"{parts[0]}:{parts[1]}";
            var signature = parts[2];

            if (signature != ComputeHmac(payload, _configuration["Security:SecretKey2"])) return false; // Проверяем подпись
            
            attemptCount = int.Parse(parts[0]);
            lastAttempt = DateTime.Parse(parts[1]);
            return true;
        }



        public string ComputeHmac(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
