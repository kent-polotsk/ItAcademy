using EFDatabase.Entities;
using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebAppGNAggregator.Models;

namespace WebAppGNAggregator.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private const int _sourceId = 1;
        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int page =1, int pageSize=5)
        {
            if (page < 1) page = 1;
            if (pageSize < 3 || pageSize>15) page = 5;
            const double minPosRate = 0;

            var totalArticlesCount = await _articleService.CountAsync(minPosRate);

            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalArticlesCount
            };

            var articles = (await _articleService.GetAllPositiveAsync(minPosRate))
                .OrderBy(a => a.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(new ArticleCollectionModel
            {
                Articles = articles,
                PageInfo = pageInfo
            });
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
    }
}
