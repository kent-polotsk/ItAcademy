using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDatabase.Entities;

namespace GNA.Services.Abstractions
{
    public interface IArticleService
    {
        public Task AddArticleAsync(Article article);
        Task<int> CountAsync(double minRate);
        public Task<Article[]> GetAllPositiveAsync(double minPositivityRate);

        public Task<Article> GetByIdAsync(Guid id);
    }
}
