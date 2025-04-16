using DAL_CQS_.Commands;
using EFDatabase;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.CommandHandlers
{
    public class AddArticlesCommandHandler : IRequestHandler<AddArticlesCommand>
    {
        private readonly GNAggregatorContext _dbContext;

        public AddArticlesCommandHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AddArticlesCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Articles.AddRangeAsync(request.Articles, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }
    }
}
