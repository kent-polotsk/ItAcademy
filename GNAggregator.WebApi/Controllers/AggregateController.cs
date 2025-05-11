using DataConvert.DTO;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GNAggregator.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregateController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly ILogger<AggregateController> _logger;

        public AggregateController(IArticleService articleService, ILogger<AggregateController> logger, ISourceService sourceService, IRssService rssService)
        {
            _articleService = articleService;
            _logger = logger;
            _sourceService = sourceService;
            _rssService = rssService;
        }

        [HttpPost]
        [ProducesResponseType<Ok>(statusCode: 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AggregateProcessing(CancellationToken cancellationToken = default)
        {
            try
            {
                var sources = await _sourceService.GetSourceWithRssAsync(cancellationToken);
                var newArticles = new List<Article>();

                foreach (var source in sources)
                {
                    var existedArticlesUrls = await _articleService.GetUniqueArticlesUrls(cancellationToken);
                    _logger.LogInformation($"{source.Name} check ok");

                    var articles = await _rssService.GetRssDataAsync(source, cancellationToken);
                    _logger.LogInformation($"{source.Name} articles loaded from rss data");

                    var newArticlesData = articles.Where(a => !existedArticlesUrls.Contains(a.Url)).ToArray();
                    newArticles.AddRange(newArticlesData);
                }

                await _articleService.AddArticlesAsync(newArticles, cancellationToken);

                await _articleService.UpdateTextForArticlesByWebScrappingAsync(cancellationToken);

                _logger.LogInformation("Articles aggregated successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during aggregating articles : {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
