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


        //[MapProperty($"{nameof(Role)}.{nameof(Role.Name)}", nameof(LoginDto.Role))]
        public partial UserDto UserToUserDto(User? user);

        //[MapProperty(nameof(UserDto.Role),$"{nameof(Role)}.{nameof(Role.Name)}")]
        public partial User UserDtoToUser(UserDto? userDto);
    }
}
