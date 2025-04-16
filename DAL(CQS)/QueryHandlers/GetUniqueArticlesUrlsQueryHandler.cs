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
    public class GetUniqueArticlesUrlsQueryHandler : IRequestHandler<GetUniqueArticlesUrlsQuery, string[]>
    {
        public readonly GNAggregatorContext _dbContext;

        public GetUniqueArticlesUrlsQueryHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string[]> Handle(GetUniqueArticlesUrlsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Select(a=>a.Url)
                .Where(a => string.IsNullOrEmpty(a)||string.IsNullOrWhiteSpace(a))
                .Distinct()
                .ToArrayAsync(cancellationToken);
        }
    }
}
