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
    public class GetPositiveArticlesWithPaginationQueryHandler : IRequestHandler<GetPositiveArticlesWithPaginationQuery, Article[]>
    {

        public readonly GNAggregatorContext _dbContext;

        public GetPositiveArticlesWithPaginationQueryHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Article[]> Handle(GetPositiveArticlesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Articles
                    .Where(article => article.PositivityRate >= request.PositivityRate&& article.PositivityRate<=5) // article.PositivityRate == null ||
                    .Include(article => article.Source)
                    .AsNoTracking()
                    .OrderByDescending(article => article.Created)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToArrayAsync(cancellationToken);

            return result;
        }
    }
}
  