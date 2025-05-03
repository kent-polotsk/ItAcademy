using MediatR;

namespace DAL_CQS_.Commands
{
    public class DeleteNotConfirmedUserCommand :IRequest<bool>
    {
        public string Email { get; set; }
    }
}
