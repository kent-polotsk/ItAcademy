using DAL_CQS_.Commands;
using EFDatabase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.CommandHandlers
{


    internal class SaveChangedArticleAsyncCommandHandler : IRequestHandler<SaveChangedArticleAsyncCommand>
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<DeleteArticleCommandHandler> _logger;

        public SaveChangedArticleAsyncCommandHandler(GNAggregatorContext dbContext, ILogger<DeleteArticleCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(SaveChangedArticleAsyncCommand request, CancellationToken cancellationToken)
        {
            var article1 = await _dbContext.Articles.FirstOrDefaultAsync(a=>a.Id.Equals(request.articleModel.Id),cancellationToken);
            if (article1 != null) 
            {
                article1.Title = request.articleModel.Title;
                article1.PositivityRate = request.articleModel.PositivityRate;
                article1.Updated= DateTime.Now;
                article1.Description = request.articleModel.Description;
                article1.Content = request.articleModel.Content;

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
