using DAL_CQS_.Queries;
using EFDatabase;
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
                .Where(a=> a.PositivityRate>=request.MinRate ) //a.PositivityRate == null ||
                .CountAsync(cancellationToken);
        }
    }
}
