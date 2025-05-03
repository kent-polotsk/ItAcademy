using DAL_CQS_.Commands;
using EFDatabase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL_CQS_.CommandHandlers
{
    internal class DeleteNotConfirmedUserCommandHandler : IRequestHandler<DeleteNotConfirmedUserCommand, bool>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<DeleteNotConfirmedUserCommandHandler> _logger;

        public DeleteNotConfirmedUserCommandHandler(GNAggregatorContext dbContext, ILogger<DeleteNotConfirmedUserCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<bool> Handle(DeleteNotConfirmedUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(request.Email));
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"{user.Email} deleted from DB");
                return true;
            }
            return false;
        }

    }
}
