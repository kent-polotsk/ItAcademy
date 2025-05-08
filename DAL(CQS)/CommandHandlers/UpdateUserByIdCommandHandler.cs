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
    internal class UpdateUserByIdCommandHandler : IRequestHandler<UpdateUserByIdCommand, bool>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<UpdateUserByIdCommandHandler> _logger;

        public UpdateUserByIdCommandHandler(GNAggregatorContext dbContext, ILogger<UpdateUserByIdCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public  async Task<bool> Handle(UpdateUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(request.userDto.Id), cancellationToken);
            if (user != null)
            {
                user.IsBanned = request.userDto.IsBanned;
                user.IsSubscribed = request.userDto.IsSubscribed;
                user.Name = request.userDto.Name;
                user.BanToDate = request.userDto.BanToDate;
                user.PositivityRate = request.userDto.PositivityRate;

                await _dbContext.SaveChangesAsync();
                return true;
                //_logger.LogInformation($"User {user.Email} updated");
            }
            //_logger.LogInformation($"User {request.userDto.Email} not found");
            return false;
        }
    }
}
