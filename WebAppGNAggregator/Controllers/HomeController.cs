using EFDatabase.Entities;
using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppGNAggregator.Filters;
using WebAppGNAggregator.Models;

namespace WebAppGNAggregator.Controllers
{
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;

        private readonly IArticleService _articleService;
        private const int _sourceId = 1;

        public HomeController(ILogger<HomeController> logger,IArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }

        [MyActionFilter]
        [HttpGet]
        public async Task<IActionResult> Index(PaginationModel paginationModel)
        {
            try
            {
                const double minPosRate = 0;
                var articles = (await _articleService.GetAllPositiveAsync(minPosRate, paginationModel.PageNumber, paginationModel.PageSize))
                    .Select(article => new ArticleModel()
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Description = article.Description,
                        Source = article.Source.Name,
                        CreationDate = article.Created,
                        Rate = article.PositivityRate ?? 0
                    })
                    .ToArray();
                _logger.LogInformation("Articles are selected");
                var totalArticlesCount = await _articleService.CountAsync(minPosRate);
                var pageInfo = new PageInfo()
                {
                    PageNumber = paginationModel.PageNumber,
                    PageSize = paginationModel.PageSize,
                    TotalItems = totalArticlesCount
                };

                ViewBag.PageSize = paginationModel.PageSize;
                ViewBag.Page = paginationModel.PageNumber;
                _logger.LogInformation("(Home)Index.Ok");
                return View(new ArticleCollectionModel
                {
                    Articles = articles,
                    PageInfo = pageInfo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article != null)
            {
                var model = new ArticleModel
                {
                    Id = article.Id,
                    Title = article.Title,
                    Description = article.Description,
                    Source = article.Source.Name,
                    CreationDate = article.Created,
                    Rate = article.PositivityRate ?? 0
                };

                return View(model);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Details2([FromRoute] Guid id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article != null)
            {
                var model = new ArticleModel
                {
                    Id = article.Id,
                    Title = article.Title,
                    Description = article.Description,
                    Source = article.Source.Name,
                    CreationDate = article.Created,
                    Rate = article.PositivityRate ?? 0
                };

                return View(model);
            }
            return NotFound();
        }


        [HttpGet]
        public async Task<IActionResult> AddArticle([FromForm] AddArticleModel? model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddArticleProcessing(AddArticleModel model)
        {
            if (!ModelState.IsValid)
                return View("AddArticle", model);

            var article = new Article
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                PositivityRate = model.Rate,
                Content = "",
                Url = "",
                Created = DateTime.Now,
                SourceId = _sourceId
            };
            _articleService.AddArticleAsync(article);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditArticle(Guid id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article != null)
            {
                var model = new ArticleModel()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Description = article.Description,
                    Source = article.Source.Name,
                    CreationDate = article.Created,
                    Rate = article.PositivityRate ?? 0
                };
                return View(model);
            }
            else { return NotFound(); }
        }
        [HttpPost]
        public async Task<IActionResult> EditArticle(ArticleModel model)
        {
            var data = model;
            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
