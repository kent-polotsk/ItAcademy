using DAL_CQS_.Queries;
using DataConvert.DTO;
using DataConvert.Models;
using GNA.Services.Abstractions;
using Jose;
using Konscious.Security.Cryptography;
using Mappers.Mappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OtpNet;
using System.Security.Cryptography;
using System.Text;

namespace GNA.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IMediator _mediator;
        private readonly UserMapper _userMapper;
        private readonly IConfiguration _config;
        private readonly ITotpCodeService _totpCodeService;

        public AccountService(ILogger<AccountService> logger, IMediator mediator, UserMapper userMapper, IConfiguration config, ITotpCodeService totpCodeService)
        {
            _logger = logger;
            _mediator = mediator;
            _userMapper = userMapper;
            _config = config;
            _totpCodeService = totpCodeService;
        }


        public async Task<LoginDto?> TryLogin(LoginModel loginModel, CancellationToken cancellationToken = default)
        {
            var salt = await _mediator.Send(new GetSaltByEmailQuery { Email = loginModel.Email }, cancellationToken);

            if (salt != null)
            {
                var passwordHash = GetPasswordHash(loginModel.Password, salt);
                var foundUser = await _mediator.Send(new TryLoginQuery { Email = loginModel.Email, PasswordHash = passwordHash }, cancellationToken);

                if (foundUser != null)
                    return _userMapper.UserToLoginDto(foundUser);
                else
                    return null;
            }
            return null;
        }

        public async Task<LoginDto?> TryRegister(RegisterModel registerModel, CancellationToken cancellationToken)
        {
            if (await _mediator.Send(new CheckUserEmailExistsQuery { Email = registerModel.Email }, cancellationToken) != null)
            {
                return null;
            }
            else
            {
                var salt = Encoding.UTF8.GetString(GenerateSalt());
                var passwordHash = GetPasswordHash(registerModel.Password, salt);
                await _mediator.Send(new TryRegisterUserCommand { Email = registerModel.Email, PasswordHash = passwordHash, PasswordSalt = salt }, cancellationToken);

                var foundUser = await _mediator.Send(new CheckUserEmailExistsQuery { Email = registerModel.Email }, cancellationToken);
                return _userMapper.UserToLoginDto(foundUser);
            }

        }


        private string GetPasswordHash(string password, string saltStored = null)
        {
            var salt = Encoding.UTF8.GetBytes(saltStored);

            if (saltStored == null)
                salt = GenerateSalt();

            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.MemorySize = 19456;
                argon2.Iterations = 2;
                argon2.DegreeOfParallelism = 1;

                var hash = argon2.GetBytes(32); // Длина хэша — 32 байта
                return Convert.ToBase64String(hash);
            }
        }


        private byte[] GenerateSalt()
        {
            var salt = new byte[16]; // 16 байт для соли
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }



        public string GenerateSecureToken(string email, int attemptsUsed = 0)
        {
            var payload = new
            {
                email,
                exp = DateTime.UtcNow.AddMinutes(5), // token lifetime
                attempts = attemptsUsed// max try count
            };

            string secretKey = _config["Security:SecretFor2faToken"];// in config
            var a = JWT.Encode(payload, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);
            return a;
        }


        public string EncryptToken(string plainToken)
        {
            byte[] key = Encoding.UTF8.GetBytes(_config["Security:SecretFor2faToken"]);
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV(); // unique IV

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] tokenBytes = Encoding.UTF8.GetBytes(plainToken);
            byte[] encryptedToken = encryptor.TransformFinalBlock(tokenBytes, 0, tokenBytes.Length);

            return Convert.ToBase64String(aes.IV) + ":" + Convert.ToBase64String(encryptedToken); // unique IV
        }



        public string DecryptToken(string encryptedToken)
        {
            string[] parts = encryptedToken.Split(":");
            byte[] iv = Convert.FromBase64String(parts[0]);
            byte[] encryptedData = Convert.FromBase64String(parts[1]);

            byte[] key = Encoding.UTF8.GetBytes(_config["Security:SecretFor2faToken"]);
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public async Task<ValidateTokenResult> ValidateSecureTokenAsync(ISession session, string? inputCode = "")
        {
            try
            {
                string secretKey = _config["Security:SecretFor2faToken"];
                var token = session.GetString("Token");
                string decodedToken = DecryptToken(token);

                var payload = JWT.Decode<Dictionary<string, object>>(decodedToken, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);
                string email = payload["email"].ToString() ?? "";
                DateTime expTime = DateTime.Parse(payload["exp"].ToString());
                int attempts = Convert.ToInt32(payload["attempts"]);


                var result = new ValidateTokenResult() { LoginDto = null, Attempts = attempts, IsCodeConfirmed = false };

                if (DateTime.UtcNow > expTime)
                {
                    result.Attempts = -1;
                    return result;
                }
                else if (attempts >= 4)
                {
                    return result;
                }
                else
                {
                    //generate totp for checking
                    var secretBytes = Encoding.UTF8.GetBytes(email + _config["Security:SecretKey1"]);
                    var totp = new Totp(secretBytes, step: 30, totpSize: 6);

                    //check code
                    bool codeIsValid = totp.VerifyTotp(inputCode, out _, new VerificationWindow(2, 1));

                    if (codeIsValid) //find user
                    {
                        var cancellationToken = new CancellationToken();
                        var user = await _mediator.Send(new CheckUserEmailExistsQuery() { Email = email }, cancellationToken);
                        if (user != null)
                        {
                            result.LoginDto = _userMapper.UserToLoginDto(user);
                            result.Attempts = attempts;
                            result.IsCodeConfirmed = true;
                        }
                    }

                    attempts++;
                    payload["attempts"] = attempts.ToString();

                    string newToken = JWT.Encode(payload, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);
                    var encryptedToken = EncryptToken(newToken);
                    session.SetString("Token", encryptedToken);

                }

                return result;
            }
            catch 
            {
                throw;
            }
        }
    }
}
