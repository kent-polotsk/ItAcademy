using Azure.Core;
using DAL_CQS_.Commands;
using EFDatabase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.CommandHandlers
{
    internal class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, bool>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<DeleteUserByIdCommandHandler> _logger;

        public DeleteUserByIdCommandHandler(GNAggregatorContext dbContext, ILogger<DeleteUserByIdCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Id.Equals(request.Id),cancellationToken);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
                //_logger.LogInformation($"User {user.Email} successfully removed from database");
                return true;
            }
            //_logger.LogInformation($"Remove failed : user {request.Id} not found");
            return false;
            }
        }
    }
