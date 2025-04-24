using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConvert.DTO
{
    public class LoginDto
    {
        public string Email { get; set; }
        //public string Password { get; set; }
        public string? Name { get; set; }
        public string Role { get; set; }
    }
}
