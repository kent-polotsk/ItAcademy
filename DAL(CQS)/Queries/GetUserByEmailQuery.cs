using EFDatabase.Entities;
using MediatR;

namespace DAL_CQS_.Queries
{
    public class GetUserByEmailQuery : IRequest<User>
    {
        public string Email { get; set; }
    }
}
