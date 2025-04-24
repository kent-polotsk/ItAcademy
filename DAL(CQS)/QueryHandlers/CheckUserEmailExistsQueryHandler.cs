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
    public class CheckUserEmailExistsQueryHandler : IRequestHandler<CheckUserEmailExistsQuery, User?>
    {
        public readonly GNAggregatorContext _dbContext;

        public CheckUserEmailExistsQueryHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> Handle(CheckUserEmailExistsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Include(u=>u.Role)
                .SingleOrDefaultAsync(u => u.Email.Equals(request.Email),cancellationToken);
        }
    }
}
