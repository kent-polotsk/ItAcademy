using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase;
using Mappers.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.QueryHandlers
{
    public class GetNewsForSendingQueryHandler : IRequestHandler<GetNewsForSendingQuery, ArticleDto?[]>
    {
        public readonly GNAggregatorContext _dbContext;
        private readonly ArticleMapper _articleMapper;

        public GetNewsForSendingQueryHandler(GNAggregatorContext dbContext, ArticleMapper articleMapper)
        {
            _dbContext = dbContext;
            _articleMapper = articleMapper;
        }
        public async Task<ArticleDto?[]> Handle(GetNewsForSendingQuery request, CancellationToken cancellationToken)
        {
            var articleDtos = await _dbContext.Articles
                .AsNoTracking()
                .Include(a=>a.Source)
                .Where(a => a.IsSent.Equals(false)).Select(a=>_articleMapper.ArticleToArticleDto(a))
                .ToArrayAsync(cancellationToken);

            return articleDtos;
        }
    }
}
