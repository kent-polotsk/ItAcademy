using DataConvert.DTO;

namespace DataConvert.Models
{
    public class ValidateTokenResult
    {
        public LoginDto? LoginDto { get; set; }
        public int Attempts { get; set; } = 0;
        public bool IsCodeConfirmed { get; set; } = false;
    }
}
