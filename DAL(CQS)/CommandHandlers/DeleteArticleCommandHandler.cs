using DAL_CQS_.Commands;
using EFDatabase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL_CQS_.CommandHandlers
{
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, bool>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<DeleteArticleCommandHandler> _logger;

        public DeleteArticleCommandHandler(GNAggregatorContext dbContext, ILogger<DeleteArticleCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var article = await _dbContext.Articles.FirstOrDefaultAsync(a => a.Id.Equals(request.id), cancellationToken);
                if (article == null)
                {
                    _logger.LogWarning($"Article with Id {request.id} not found.");
                    return false;
                }

                _dbContext.Articles.Remove(article);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Article with Id {request.id} successfully deleted.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the article.");
                throw;
            }
        }
    }
}
