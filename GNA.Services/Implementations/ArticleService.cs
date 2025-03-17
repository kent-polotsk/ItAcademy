using EFDatabase;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ArticleService> _logger;


        public ArticleService(GNAggregatorContext dbContext, ILogger<ArticleService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddArticleAsync(Article article)
        {
            await _dbContext.Articles.AddAsync(article);
            await _dbContext.SaveChangesAsync();
        }

        async Task<Article[]> IArticleService.GetAllPositiveAsync(double minPositivityRate, int pageNumber, int pageSize)
        {
            try
            {
                var result = await _dbContext.Articles
                    .Where(article => article.PositivityRate >= minPositivityRate)
                    .Include(article => article.Source)
                    .AsNoTracking()
                    .OrderBy(article => article.PositivityRate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToArrayAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
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
