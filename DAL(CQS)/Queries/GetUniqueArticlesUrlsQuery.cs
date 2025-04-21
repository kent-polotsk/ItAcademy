using MediatR;

namespace DAL_CQS_.Queries
{
    public class GetUniqueArticlesUrlsQuery : IRequest<string[]>
    {
    }
}
