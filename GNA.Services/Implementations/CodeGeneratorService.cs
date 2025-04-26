using GNA.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNA.Services.Implementations
{
    public class CodeGeneratorService : ICodeGeneratorService
    {
        public CodeGeneratorService() { }

        public string GenerateCode()
        {
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var codeBytes = new byte[4]; 
            rng.GetBytes(codeBytes);
            var result =  (BitConverter.ToUInt32(codeBytes, 0)%1000000).ToString("D6");

            return result;
        }
    }
}
