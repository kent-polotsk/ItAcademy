using DAL_CQS_.Commands;
using EFDatabase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.CommandHandlers
{
    public class UpdateTextForArticlesCommandHandler : IRequestHandler<UpdateTextForArticlesCommand>
    {
        private readonly GNAggregatorContext _dbContext;

        public UpdateTextForArticlesCommandHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Handle(UpdateTextForArticlesCommand request, CancellationToken cancellationToken)
        {
            foreach (var keyValuePair in request.Data)
            {
                var article = await _dbContext.Articles.SingleOrDefaultAsync(a=>a.Id.Equals(keyValuePair.Key),cancellationToken);
                if (article != null)
                {
                    article.Content = keyValuePair.Value;
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
