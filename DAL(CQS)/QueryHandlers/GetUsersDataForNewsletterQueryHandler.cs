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
    internal class GetUsersDataForNewsletterQueryHandler : IRequestHandler<GetUsersDataForNewsletterQuery, Dictionary<string, double>>
    {
        public readonly GNAggregatorContext _dbContext;
        private readonly ArticleMapper _articleMapper;

        public GetUsersDataForNewsletterQueryHandler(GNAggregatorContext dbContext, ArticleMapper articleMapper)
        {
            _dbContext = dbContext;
            _articleMapper = articleMapper;
        }

        public async Task<Dictionary<string, double>> Handle(GetUsersDataForNewsletterQuery request, CancellationToken cancellationToken)
        {

            Dictionary<string,double> userData = new Dictionary<string, double>();
            var users = await _dbContext.Users.AsNoTracking().Where(u=>u.IsSubscribed.Equals(true)).ToListAsync(cancellationToken);

            foreach (var user in users)
            {
                userData.Add(user.Email,user.PositivityRate);
            }

            return userData;
        }
    }
}
