using DAL_CQS_.Queries;
using EFDatabase.Entities;
using EFDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace DAL_CQS_.QueryHandlers
{
    public class CheckUserEmailExistsQueryHandler : IRequestHandler<CheckUserEmailExistsQuery, bool>
    {
        public readonly GNAggregatorContext _dbContext;

        public CheckUserEmailExistsQueryHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CheckUserEmailExistsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email.Equals(request.Email),cancellationToken);
        }
    }
}
