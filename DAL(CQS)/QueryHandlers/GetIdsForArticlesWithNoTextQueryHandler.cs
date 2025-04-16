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
    public class GetIdsForArticlesWithNoTextQueryHandler : IRequestHandler<GetIdsForArticlesWithNoTextQuery, Guid[]>
    {

        public readonly GNAggregatorContext _dbContext;

        public GetIdsForArticlesWithNoTextQueryHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid[]> Handle(GetIdsForArticlesWithNoTextQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles.AsNoTracking()
                .Where(a => !string.IsNullOrWhiteSpace(a.Url) && null==a.Content)//string.IsNullOrWhiteSpace(a.Content)
                .Select(a => a.Id)
                .ToArrayAsync(cancellationToken);
        }
    }
}
