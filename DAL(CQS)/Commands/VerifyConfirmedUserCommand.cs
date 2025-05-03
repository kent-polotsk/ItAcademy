using MediatR;

namespace DAL_CQS_.Commands
{
    public class VerifyConfirmedUserCommand : IRequest<bool>
    {
        public string? Email { get; set; }
    }
}
