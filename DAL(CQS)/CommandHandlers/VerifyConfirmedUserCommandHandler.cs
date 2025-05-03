using DAL_CQS_.Commands;
using EFDatabase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL_CQS_.CommandHandlers
{
    public class VerifyConfirmedUserCommandHandler : IRequestHandler<VerifyConfirmedUserCommand, bool>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<VerifyConfirmedUserCommandHandler> _logger;

        public VerifyConfirmedUserCommandHandler(GNAggregatorContext dbContext, ILogger<VerifyConfirmedUserCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<bool> Handle(VerifyConfirmedUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(request.Email));
            if (user != null)
            {
                user.IsVerified = true;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"{user.Email} is verified in DB");
                return true;
            }
            return false;
        }
    }
}
