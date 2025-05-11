using EFDatabase.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.Commands
{
    public class SaveRatedArticlesCommand : IRequest<bool>
    {
        public Article[] Articles { get; set; }
    }
}
