using DataConvert.DTO;
using MediatR;

namespace DAL_CQS_.Queries
{
    public class GetArticleByIdQuery : IRequest<ArticleDto?>
    {
        public Guid Id { get; set; }

    }
}
