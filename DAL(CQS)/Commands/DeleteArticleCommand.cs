using MediatR;

namespace DAL_CQS_.Commands
{
    public class DeleteArticleCommand : IRequest<bool>
    {
        public Guid id { get; set; }
    }
}
