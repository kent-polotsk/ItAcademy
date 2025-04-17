using DAL_CQS_.Commands;
using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Mappers.Mappers;

namespace GNA.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly GNAggregatorContext _dbContext;
        private readonly ILogger<ArticleService> _logger;
        private readonly IMediator _mediator;
        private readonly ArticleMapper _articleMapper;

        public ArticleService(GNAggregatorContext dbContext, ILogger<ArticleService> logger, IMediator mediator, ArticleMapper articleMapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mediator = mediator;
            _articleMapper = articleMapper;
        }


        public async Task<ArticleDto?[]> GetAllPositiveAsync(double? minPositivityRate, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var articleDtos = (await _mediator.Send(new GetPositiveArticlesWithPaginationQuery()
                {
                    PositivityRate = minPositivityRate,
                    Page = pageNumber,
                    PageSize = pageSize
                }, cancellationToken))
                    .Select(article => _articleMapper.ArticleToArticleDto(article))
                    .ToArray();

                _logger.LogInformation("Articles are selected and converted to ArticleDtos");
                return articleDtos;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Articles getting error: {ex.Message}");
                throw;
            }
        }






        public async Task<ArticleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var article = await _mediator.Send(new GetArticleByIdQuery() { Id = id }, cancellationToken);
            return article;

        }



        public async Task<int> CountAsync(double minRate, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new CountArticlesQuery() { }, cancellationToken);
        }


        public async Task<string[]> GetUniqueArticlesUrls(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetUniqueArticlesUrlsQuery() { }, cancellationToken);
        }



        public async Task AddArticlesAsync(IEnumerable<Article> newUniqueArticles, CancellationToken cancellationToken)
        {
            await _mediator.Send(new AddArticlesCommand() { Articles = newUniqueArticles }, cancellationToken);
        }


        public async Task UpdateContentByWebScrappingAsync(Guid[] ids, CancellationToken cancellationToken = default)
        {

        }


        public async Task UpdateTextForArticlesByWebScrappingAsync(CancellationToken cancellationToken = default)
        {
            var ids = await _mediator.Send(new GetIdsForArticlesWithNoTextQuery(), cancellationToken);
            var dictionary = new Dictionary<Guid, string>();

            foreach (var id in ids)
            {
                var article = await _mediator.Send(new GetArticleByIdQuery { Id = id }, cancellationToken);

                if (article == null || string.IsNullOrWhiteSpace(article.Url))
                {
                    _logger.LogWarning($"*ArticleService* Article {id} not found or URL doesn't exist");
                    continue;
                }
                else
                {
                    var web = new HtmlWeb();
                    var doc = await web.LoadFromWebAsync(article.Url, cancellationToken);

                    if (doc == null)
                    {
                        _logger.LogWarning($"*ArticleService* Unable to load document from {article.Url} *ArticleService");
                        continue;
                    }


                    HtmlNode articleNode = null;

                    if (article.Url.Contains("onliner"))
                    {
                        articleNode = doc.DocumentNode.SelectSingleNode("//div[@class='news-text']");
                    }
                    else if (article.Url.Contains("belta"))
                    {
                        articleNode = doc.DocumentNode.SelectSingleNode("//div[@class='js-mediator-article']");
                    }
                    else
                    {
                        articleNode = doc.DocumentNode.SelectSingleNode("//div[@class='article__text']");
                    }


                    if (articleNode == null)
                    {
                        _logger.LogWarning($"*ArticleService* Unable to load data from {article.Url}");
                        continue;
                    }
                    string innerText = articleNode.InnerText;
                    string readyText = Regex.Replace(innerText, @"\s+", " ").Trim();
                    dictionary.Add(id, readyText);

                }
                await _mediator.Send(new UpdateTextForArticlesCommand()
                {
                    Data = dictionary
                }, cancellationToken);
            }
        }

        public async Task<bool> DeleteAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new DeleteArticleCommand() { id = Id }, cancellationToken);
        }

        public Task SaveChangedArticleAsync(ArticleDto atricleDto, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new SaveChangedArticleAsyncCommand() { articleDto = atricleDto }, cancellationToken);
        }

        public async Task<object?> GetAllPositiveAsync(int minRate, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
