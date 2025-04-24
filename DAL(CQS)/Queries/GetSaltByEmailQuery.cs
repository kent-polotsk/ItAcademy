using EFDatabase.Entities;
using MediatR;

namespace DAL_CQS_.Queries
{
    public class GetSaltByEmailQuery : IRequest<string?>
    {
        public string Email { get; set; }
        //public string PasswordHash { get; set; }
    }
}
