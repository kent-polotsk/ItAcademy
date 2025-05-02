using System.Security.Cryptography;
using System.Text;
using GNA.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using OtpNet;

namespace GNA.Services.Implementations
{
    public class TotpCodeService : ITotpCodeService
    {
        private readonly IConfiguration _configuration;

        public TotpCodeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateTotpCode(string email)
        {
            //var secretBytes = new HMACSHA1(Encoding.UTF8.GetBytes(_configuration["Security:SecretKey1"])).ComputeHash(Encoding.UTF8.GetBytes(email));
            var secretBytes = Encoding.UTF8.GetBytes(email + _configuration["Security:SecretKey1"]);
            var totp = new Totp(secretBytes, step: 30, totpSize: 6);
           
            var code = totp.ComputeTotp();
            //Console.WriteLine("HMAC_____"+Convert.ToBase64String(secretBytes));
            //Console.WriteLine("NO__HMAC_"+Convert.ToBase64String(secretBytes));
            return code; // Генерируем 6-значный код
        }

        //public string ComputeHmac(string data, string key)
        //{
        //    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        //    var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        //    return Convert.ToBase64String(hashBytes);
        //}
    }
}
