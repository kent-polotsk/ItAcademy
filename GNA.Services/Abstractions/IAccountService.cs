using DataConvert.DTO;
using EFDatabase.Entities;


namespace GNA.Services.Abstractions
{
    public interface IAccountService
    {

         Task<bool> TryLogin(LoginModel loginModel, CancellationToken cancellationToken);
    }
}
