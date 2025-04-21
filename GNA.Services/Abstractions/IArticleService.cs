using DataConvert.DTO;
using EFDatabase.Entities;


namespace GNA.Services.Abstractions
{
    public interface IArticleService
    {

        public Task<int> CountAsync(double minRate, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        
        public Task<ArticleDto?[]> GetAllPositiveAsync(double? minPositivityRate, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        public Task<ArticleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<string[]> GetUniqueArticlesUrls(CancellationToken cancellationToken = default);

        public Task AddArticlesAsync(IEnumerable<Article> newUniqueArticles, CancellationToken cancellationToken = default);

       // public Task UpdateContentByWebScrappingAsync(Guid[] ids, CancellationToken token = default);
        public Task UpdateTextForArticlesByWebScrappingAsync(CancellationToken cancellationToken);
        
        public Task SaveChangedArticleAsync(ArticleDto articleDto, CancellationToken token=default);
    }
}
