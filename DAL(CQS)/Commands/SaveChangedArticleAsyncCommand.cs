using DataConvert.DTO;
using MediatR;


namespace DAL_CQS_.Commands
{
    public class SaveChangedArticleAsyncCommand : IRequest
    {
        public ArticleDto articleDto { get; set; }
    }
}
