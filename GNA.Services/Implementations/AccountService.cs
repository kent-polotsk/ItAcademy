using DAL_CQS_.Queries;
using GNA.Services.Abstractions;
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
        //private readonly ArticleMapper _articleMapper;

        public AccountService(ILogger<AccountService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            //_articleMapper = articleMapper;
        }

        public async Task<bool> TryLogin(LoginModel loginModel, CancellationToken cancellationToken = default)
        {
            var passwordHash = GetPasswordHash(loginModel.Password);
            var foundUser = await _mediator.Send(new TryLoginQuery { Email = loginModel.Email, PasswordHash = passwordHash }, cancellationToken);

            return foundUser;
        }

        public async Task<bool> TryRegister(RegisterModel registerModel, CancellationToken cancellationToken)
        {
            if (await _mediator.Send(new CheckUserEmailExistsQuery { Email = registerModel.Email }, cancellationToken))
            {
                return false;
            }
            else
            {
                var passwordHash = GetPasswordHash(registerModel.Password);
                await _mediator.Send(new TryRegisterUserCommand { Email = registerModel.Email, PasswordHash = passwordHash }, cancellationToken);
                return true;
            }

        }

        public string GetPasswordHash(string password)
        {
            using var sha256 = SHA256.Create();

            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-","").ToLower();
        }
    }
}
