using EFDatabase.Entities;
using MediatR;

namespace DAL_CQS_.Queries
{
    public class GetAllSourcesWithRssQuery : IRequest<Source[]>
    {
    }
}
