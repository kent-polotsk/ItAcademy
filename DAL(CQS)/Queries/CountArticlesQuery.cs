using Azure.Core;
using MediatR;

namespace DAL_CQS_.Queries
{
    public class CountArticlesQuery : IRequest<int>
    {
        public double MinRate { get; set; }
    }
}
