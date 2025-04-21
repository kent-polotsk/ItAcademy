using MediatR;

namespace DAL_CQS_.Commands
{
    public class UpdateTextForArticlesCommand : IRequest
    {
        public Dictionary<Guid,string> Data { get; set; }
    }
}
