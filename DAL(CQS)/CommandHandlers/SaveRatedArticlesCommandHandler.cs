using DAL_CQS_.Commands;
using EFDatabase;
using Mappers.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.CommandHandlers
{
    public class SaveRatedArticlesCommandHandler : IRequestHandler<SaveRatedArticlesCommand, bool>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<SaveRatedArticlesCommandHandler> _logger;
        private readonly ArticleMapper _articleMapper;

        public SaveRatedArticlesCommandHandler(GNAggregatorContext dbContext, ILogger<SaveRatedArticlesCommandHandler> logger, ArticleMapper articleMapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _articleMapper = articleMapper;
        }
        public async Task<bool> Handle(SaveRatedArticlesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Articles.UpdateRange(request.Articles);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable save articles in db: {ex.Message}");
                return false;
            }
        }
    }
}
