using MediatR;

namespace DAL_CQS_.Queries
{
    public class GetIdsForArticlesWithNoTextQuery : IRequest<Guid[]>
    {

    }
}
