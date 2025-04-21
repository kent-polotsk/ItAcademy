using DAL_CQS_.Queries;
using GNA.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

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

        public async Task<bool> TryLogin(LoginModel loginModel, CancellationToken cancellationToken= default)
        {
            var foundUser = await _mediator.Send(new TryLoginQuery { LoginModel=loginModel} ,  cancellationToken);

            return foundUser;
        }
    }
}
