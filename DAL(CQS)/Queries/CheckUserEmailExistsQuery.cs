using EFDatabase.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.Queries
{
    public class CheckUserEmailExistsQuery : IRequest<User?>
    {
        public string Email { get; set; }
    }
}
