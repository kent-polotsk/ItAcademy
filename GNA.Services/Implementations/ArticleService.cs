using EFDatabase;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNA.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly GNAggregatorContext _dbContext;


        public ArticleService(GNAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddArticleAsync(Article article)
        {
            await _dbContext.Articles.AddAsync(article);
            await _dbContext.SaveChangesAsync();
        }

        async Task<Article[]> IArticleService.GetAllPositiveAsync(double minPositivityRate)
        {
            return await _dbContext.Articles
                .Where(article => article.PositivityRate >= minPositivityRate)
                .Include(article => article.Source)
                .AsNoTracking()
                .ToArrayAsync();
        }

        async Task<Article?> IArticleService.GetByIdAsync(Guid id)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Include(article => article.Source)
                .FirstOrDefaultAsync(article => article.Id.Equals(id));
        }

        async Task<int> IArticleService.CountAsync(double minRate)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .CountAsync();
        }
    }
}
