using Azure.Core;
using DAL_CQS_.Queries;
using EFDatabase;
using EFDatabase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.QueryHandlers
{
    public class GetSaltByEmailQueryHandler : IRequestHandler<GetSaltByEmailQuery, string?>
    {
        private readonly GNAggregatorContext _dbContext;

        public GetSaltByEmailQueryHandler(GNAggregatorContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public async Task<string?> Handle(GetSaltByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var foundUser = await _dbContext.Users
                    .AsNoTracking()
                    //.Include(x => x.Role)
                    .SingleOrDefaultAsync(u => u.Email.Equals(request.Email), cancellationToken);

                return foundUser!=null?foundUser.PasswordSalt:null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
