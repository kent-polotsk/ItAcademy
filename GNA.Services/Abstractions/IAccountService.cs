using DataConvert.DTO;
using DataConvert.Models;
using EFDatabase.Entities;
using Microsoft.AspNetCore.Http;


namespace GNA.Services.Abstractions
{
    public interface IAccountService
    {
        Task<LoginDto?> TryLogin(LoginModel loginModel, CancellationToken cancellationToken);

        Task<LoginDto?> TryRegister(RegisterModel registerModel, CancellationToken cancellationToken);

        string GenerateSecureToken(string email, int attemptsUsed);

        string EncryptToken(string plainToken);

        Task<ValidateTokenResult> ValidateSecureTokenAsync(ISession session, string? inputCode);

        string DecryptToken(string encryptedToken);
    }
}
