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
    public class MarkSentNewsCommandHandler : IRequestHandler<MarkSentNewsCommand>
    {
        private readonly GNAggregatorContext _dbContext;

        public MarkSentNewsCommandHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(MarkSentNewsCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Articles
                .Where(a => request.ArticleIds
                .Contains(a.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.IsSent, true), cancellationToken);           
        }
    }
}
