using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase;
using EFDatabase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppGNAggregator.Mappers;

namespace DAL_CQS_.QueryHandlers
{
    public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto?>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ArticleMapper _articleMapper;


        public GetArticleByIdQueryHandler(GNAggregatorContext dbContext, ArticleMapper articleMapper)
        {
            _dbContext = dbContext;
            _articleMapper = articleMapper;
        }

        public async Task<ArticleDto?> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _dbContext.Articles
                .Include(a => a.Source)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(request.Id));
            
            return article!=null
                ?_articleMapper.ArticleToArticleDto(article)
                : null;
        }
    }
}
