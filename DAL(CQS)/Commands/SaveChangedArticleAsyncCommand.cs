using MediatR;


namespace DAL_CQS_.Commands
{
    public class SaveChangedArticleAsyncCommand : IRequest
    {
        public ArticleModel articleModel { get; set; }
    }
}
