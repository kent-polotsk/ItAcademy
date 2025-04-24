using EFDatabase.Entities;
using Riok.Mapperly.Abstractions;
using DataConvert.DTO;
using System.Linq;

namespace Mappers.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        [MapProperty($"{nameof(Role)}.{nameof(Role.Name)}", nameof(LoginDto.Role))] 
        public partial LoginDto UserToLoginDto(User? user);

       
    }
}
