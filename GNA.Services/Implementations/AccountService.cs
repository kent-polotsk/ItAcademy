using DAL_CQS_.Queries;
using DataConvert.DTO;
using GNA.Services.Abstractions;
using Konscious.Security.Cryptography;
using Mappers.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace GNA.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IMediator _mediator;
        private readonly UserMapper _userMapper;

        public AccountService(ILogger<AccountService> logger, IMediator mediator, UserMapper userMapper)
        {
            _logger = logger;
            _mediator = mediator;
            _userMapper = userMapper;
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
                var salt = Encoding.UTF8.GetString( GenerateSalt());
                var passwordHash = GetPasswordHash(registerModel.Password, salt);
                await _mediator.Send(new TryRegisterUserCommand { Email = registerModel.Email, PasswordHash = passwordHash ,PasswordSalt = salt}, cancellationToken);

                var foundUser = await _mediator.Send(new CheckUserEmailExistsQuery { Email = registerModel.Email }, cancellationToken);
                return _userMapper.UserToLoginDto(foundUser);
                //return loginDto;
            }

        }


        public string GetPasswordHash(string password, string saltStored = null)
        {

            var salt = Encoding.UTF8.GetBytes(saltStored);

            if (saltStored == null)
                salt = GenerateSalt();

            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt; // Соль (должна быть уникальной для каждого пароля)
                argon2.MemorySize = 19456; // Количество памяти (в килобайтах)
                argon2.Iterations = 2; // Количество итераций
                argon2.DegreeOfParallelism = 1; // Параллельность (количество потоков)

                var hash = argon2.GetBytes(32); // Длина хэша — 32 байта
                return Convert.ToBase64String(hash);
            }
        }


        public byte[] GenerateSalt()
        {
            var salt = new byte[16]; // 16 байт для соли
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

    }
}
