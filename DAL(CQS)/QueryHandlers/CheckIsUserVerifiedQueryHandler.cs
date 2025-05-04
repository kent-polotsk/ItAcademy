using DAL_CQS_.Queries;
using EFDatabase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL_CQS_.QueryHandlers
{
    public class CheckIsUserVerifiedQueryHandler : IRequestHandler<CheckIsUserVerifiedQuery, bool>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<CheckIsUserVerifiedQueryHandler> _logger;

        public CheckIsUserVerifiedQueryHandler(GNAggregatorContext dbContext, ILogger<CheckIsUserVerifiedQueryHandler> logger)
        { 
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> Handle(CheckIsUserVerifiedQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(request.Email));
            if (user != null)
            {
                _logger.LogInformation($"User {request.Email} checked");
                return user.IsVerified;
            }
            _logger.LogWarning($"User {request.Email} not found");
            return false;
        }
    }
}
