using DAL_CQS_.Queries;
using EFDatabase;
using EFDatabase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DAL_CQS_.QueryHandlers
{
    internal class TryRegisterUserCommandHandler : IRequestHandler<TryRegisterUserCommand>
    {
        private readonly GNAggregatorContext _dbContext;

        public TryRegisterUserCommandHandler(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task Handle(TryRegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _dbContext.Roles.AsNoTracking().SingleOrDefaultAsync(r => r.Name.Equals("User"), cancellationToken);

                if (role != null)
                {
                    await _dbContext.Users.AddAsync(new User()
                    {
                        Id = Guid.NewGuid(),
                        Email = request.Email,
                        PasswordHash = request.PasswordHash,
                        PasswordSalt = request.PasswordSalt,
                        CreatedDate = DateTime.Now,
                        RoleId = role.Id
                    }, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException("Role not found");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Can't register new user");
            }
        }
    }
}
