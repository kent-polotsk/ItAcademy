using DAL_CQS_.Queries;
using EFDatabase.Entities;
using EFDatabase;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DAL_CQS_.QueryHandlers
{
    public class CheckUserEmailExistsQueryHandler : IRequestHandler<CheckUserEmailExistsQuery, User?>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<CheckUserEmailExistsQueryHandler> _logger;

        public CheckUserEmailExistsQueryHandler(GNAggregatorContext dbContext, ILogger<CheckUserEmailExistsQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<User?> Handle(CheckUserEmailExistsQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Email.Equals(request.Email), cancellationToken);

            if (user != null)
            {
                //_logger.LogInformation($"User {user.Email} found");
                return user;
            }
            else
            {
                //_logger.LogWarning($"User {request.Email} not found!");
                return null;
            }
        }
    }
}
