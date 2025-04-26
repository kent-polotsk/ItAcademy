using DAL_CQS_.Queries;
using EFDatabase;
using EFDatabase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.QueryHandlers
{
    public class GetAllSourcesWithRssQueryHandler : IRequestHandler<GetAllSourcesWithRssQuery, Source[]>
    {
        public readonly GNAggregatorContext _dbContext;

        public GetAllSourcesWithRssQueryHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Source[]> Handle(GetAllSourcesWithRssQuery request, CancellationToken cancellationToken)
        {

            return await _dbContext.Sources
                .AsNoTracking()
                .Where(s => !string.IsNullOrEmpty(s.RSSURL)) 
                .ToArrayAsync();
        }
    }
}
