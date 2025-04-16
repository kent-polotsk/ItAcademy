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
    public class CountArticlesQueryHandler : IRequestHandler<CountArticlesQuery,int>
    {
        public readonly GNAggregatorContext _dbContext;

        public CountArticlesQueryHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CountArticlesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }
    }
}
