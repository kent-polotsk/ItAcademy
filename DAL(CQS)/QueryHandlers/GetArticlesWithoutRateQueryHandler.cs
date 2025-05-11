using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase;
using EFDatabase.Entities;
using Mappers.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DAL_CQS_.QueryHandlers
{
    public class GetArticlesWithoutRateQueryHandler : IRequestHandler<GetArticlesWithoutRateQuery, Article?[]>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ArticleMapper _articleMapper;

        public GetArticlesWithoutRateQueryHandler(GNAggregatorContext dbContext, ArticleMapper articleMapper)
        {
            _dbContext = dbContext;
            _articleMapper = articleMapper;
        }

        public async Task<Article?[]> Handle(GetArticlesWithoutRateQuery request, CancellationToken cancellationToken)
        {
            var articles = await _dbContext.Articles
                //.AsNoTracking()
                .Include(a => a.Source)
                .Where(a => a.PositivityRate == null)
                .Take(50)
                //.Select(a => _articleMapper.ArticleToArticleDto(a))
                .ToArrayAsync(cancellationToken);

            return articles;
        }
    }
}
