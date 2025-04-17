using DAL_CQS_.Commands;
using EFDatabase;
using Mappers.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL_CQS_.CommandHandlers
{


    internal class SaveChangedArticleAsyncCommandHandler : IRequestHandler<SaveChangedArticleAsyncCommand>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<DeleteArticleCommandHandler> _logger;
        private readonly ArticleMapper _articleMapper;

        public SaveChangedArticleAsyncCommandHandler(GNAggregatorContext dbContext, ILogger<DeleteArticleCommandHandler> logger, ArticleMapper articleMapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _articleMapper = articleMapper;
        }

        public async Task Handle(SaveChangedArticleAsyncCommand request, CancellationToken cancellationToken)
        {
            var article = await _dbContext.Articles
                .Include(article => article.Source)
                .Include(a=>a.Comments)
                .FirstOrDefaultAsync(a => a.Id.Equals(request.articleDto.Id), cancellationToken);
                
            if (article != null)
            {
               // var source = article.Source;
                _articleMapper.UpdateArticleFromDto(request.articleDto, article);

               // article.Source = source;
                _logger.LogInformation("Dto converted to article");
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogError("Can't save changes: article dto is null");
            }
        }
    }
}
