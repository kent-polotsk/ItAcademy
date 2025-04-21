using EFDatabase.Entities;
using MediatR;

namespace DAL_CQS_.Commands
{
    public class AddArticlesCommand : IRequest
    {
        public IEnumerable<Article> Articles { get; set; }
    }
}
