using EFDatabase;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataConvert.DTO;
using Serilog;
using Mappers.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace WebAppGNAggregator.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly GNAggregatorContext _dbContext;

        //private const int _sourceId = 1;

        private readonly ArticleMapper _articleMapper;


        public ArticlesController(ILogger<HomeController> logger,
            IArticleService articleService, 
            ISourceService sourceService,
            IRssService rssService, 
            ArticleMapper articleMapper, 
            GNAggregatorContext dbContext)
        {
            _logger = logger;
            _articleService = articleService;
            _articleMapper = articleMapper;
            _sourceService = sourceService;
            _rssService = rssService;
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            _logger.LogInformation("Redirected to Home/Index");
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Aggregate()
        {
            _logger.LogInformation("View 'Aggregate' loaded");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AggregateProcessing(CancellationToken cancellationToken = default)
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
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task Rate(CancellationToken cancellationToken = default) 
        {
            var articles = await _dbContext.Articles.Take(10).ToListAsync(cancellationToken);

            foreach (var a in articles)
            {
                if (a.Content!=null)
                a.PositivityRate = _articleService.PositivityRating(a.Content);
            }
            await _dbContext.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var articleDto = await _articleService.GetByIdAsync(id);

                if (articleDto != null)
                {
                    var articleModel = _articleMapper.ArticleDtoToArticleModel(articleDto);
                    
                    _logger.LogInformation($"Article {articleModel.Id} retrieved successfully.");
                    return View(articleModel);
                }
                else
                {
                    _logger.LogWarning($"Article {id} not found.");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 404;
                return RedirectToAction("Error", "Home", new
                {
                    statusCode = HttpContext.Response.StatusCode,
                    errorMessage = $"Такой новости нет :(<br>Почитайте что-нибудь ещё.<br><br>{ex.Message}"
                });
            }
        }



        [HttpPost]
        public async Task<IActionResult> Delete(ArticleDto model)
        {
            try
            {
                var isDeleted = await _articleService.DeleteAsync(model.Id);

                if (isDeleted == true)
                {
                    _logger.LogInformation($"Article {model.Id} deleted successfully");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning($"Article {model.Id} not found");
                    //return RedirectToAction("Error", "Home", new { ErrorMessage = "Такой новости нет :(<br>Почитайте что-нибудь ещё", HttpContext.Response.StatusCode });
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 404;
                return RedirectToAction("Error", "Home", new
                {
                    statusCode = HttpContext.Response.StatusCode,
                    errorMessage = "Такой новости нет :(<br>Почитайте что-нибудь ещё."
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditArticle(Guid id)
        {
            try
            {
                var articleDto = await _articleService.GetByIdAsync(id);

                if (articleDto != null)
                {
                    var articleModel = _articleMapper.ArticleDtoToArticleModel(articleDto);
                    _logger.LogInformation($"Article {articleModel.Id} loaded for editing successfully");
                    return View(articleModel);
                }
                else
                {
                    _logger.LogWarning($"Article {id} for editing not found");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 404;
                return RedirectToAction("Error", "Home", new
                {
                    statusCode = HttpContext.Response.StatusCode,
                    errorMessage = "Такой новости нет :(<br>Почитайте что-нибудь ещё."
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditArticle(ArticleModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Article edited model is not valid");
                    return View(model);
                }

                else
                {
                    var editedModel = _articleMapper.ArticleModelToArticleDto(model);
                    editedModel.Updated = DateTime.Now;
                    await _articleService.SaveChangedArticleAsync(editedModel);
                    _logger.LogInformation($"Article {model.Id} edited successfully");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 404;
                return RedirectToAction("Error", "Home", new
                {
                    statusCode = HttpContext.Response.StatusCode,
                    errorMessage = "Такой новости нет :(<br>Почитайте что-нибудь ещё."
                });
            }
        }
    }
}
