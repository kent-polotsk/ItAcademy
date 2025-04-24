using DataConvert.DTO;
using EFDatabase.Entities;


namespace GNA.Services.Abstractions
{
    public interface IAccountService
    {
        Task<LoginDto?> TryLogin(LoginModel loginModel, CancellationToken cancellationToken);

        Task<LoginDto?> TryRegister(RegisterModel registerModel, CancellationToken cancellationToken);
    }
}
