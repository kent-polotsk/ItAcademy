namespace GNA.Services.Abstractions
{
    public interface ITotpCodeService
    {
        string GenerateTotpCode(string email);

        //string GenerateToken(int attemptCount, DateTime lastAttempt);

        //bool ValidateToken(string token, out int attemptCount, out DateTime lastAttempt);

        //string ComputeHmac(string data, string key);
    }
}
