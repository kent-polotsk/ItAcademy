using GNA.Services.Abstractions;

namespace GNA.Services.Implementations
{
    public class AuthorsService : IAuthorsService
    {
        private readonly string someSecretValue1;
        private readonly string someSecretValue2;

        public AuthorsService()
        {
            someSecretValue1 = "some_string1";
            someSecretValue2 = "some_string2";
        }

    }
}