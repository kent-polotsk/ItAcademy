using DataConvert.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.Commands
{
    public class UpdateUserByIdCommand : IRequest<bool>
    {
        public UserDto userDto { get; set; }
    }
}
