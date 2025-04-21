using EFDatabase.Entities;
using MediatR;

namespace DAL_CQS_.Queries
{
    public class GetPositiveArticlesWithPaginationQuery : IRequest<Article[]>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public double? PositivityRate { get; set; }
    }
}
