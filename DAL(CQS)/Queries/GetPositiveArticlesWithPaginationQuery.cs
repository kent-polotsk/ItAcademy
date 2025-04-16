using EFDatabase.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.Queries
{
    public class GetPositiveArticlesWithPaginationQuery : IRequest<Article[]>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public double? PositivityRate { get; set; }
    }
}
