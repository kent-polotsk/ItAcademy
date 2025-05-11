
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using GNA.Services.Abstractions;
using System.Threading;
using DataConvert.DTO;

namespace GNAggregator.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleService articleService, ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        /// <summary>
        /// Get articles accordind min rate with pagination
        /// </summary>
        /// <param name="minRate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Array of articles</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllArticles(double? minRate, int pageNumber = 1, int pageSize = 15, CancellationToken cancellationToken = default)
        {
            var articles = await _articleService.GetAllPositiveAsync(minRate, pageNumber, pageSize, cancellationToken);
            _logger.LogInformation($"Articles (minRate={minRate}, pNumber={pageNumber}, p.Size={pageSize}) successfully loaded");
            return Ok(articles);
        }

        /// <summary>
        /// Get article
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>One article by Id</returns>
        [HttpGet("id")]
        [ProducesResponseType<ArticleDto>(statusCode:200)]
        [ProducesResponseType<NotFoundResult>(statusCode:404)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(id, cancellationToken);
                if (article == null)
                {
                    _logger.LogWarning($"Article {id} not found");
                    return NotFound();
                }
                _logger.LogInformation($"Article {id} successfully found");
                return Ok(article);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting article by id");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPatch("id")]
        public async Task<IActionResult> Update(ArticleDto articleDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await _articleService.SaveChangedArticleAsync(articleDto, cancellationToken);
                _logger.LogInformation($"Article {articleDto.Id} successfully updated");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while saving article");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _articleService.DeleteAsync(id, cancellationToken);
                _logger.LogInformation($"Article {id} successfully deleted");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during delete article by id");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
