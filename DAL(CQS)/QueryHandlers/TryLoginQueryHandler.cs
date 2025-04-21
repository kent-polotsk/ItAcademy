using Azure.Core;
using DAL_CQS_.Queries;
using EFDatabase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.QueryHandlers
{
    internal class TryLoginQueryHandler : IRequestHandler<TryLoginQuery, bool>
    {
        private readonly GNAggregatorContext _dbContext;

        public TryLoginQueryHandler(GNAggregatorContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public async Task<bool> Handle(TryLoginQuery request, CancellationToken cancellationToken)
        {
            var foundUser = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Email.Equals(request.LoginModel.Email),cancellationToken);
            return foundUser != null && request.LoginModel.PasswordHash.Equals(foundUser.PasswordHash);
        }
    }
}
