using DataConvert.DTO;
using EFDatabase.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_CQS_.Queries
{
    public class GetArticleByIdQuery : IRequest<ArticleDto?>
    {
        public Guid Id { get; set; }

    }
}
