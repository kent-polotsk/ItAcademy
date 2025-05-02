using EFDatabase;
using EFDatabase.Entities;
using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Mappers.Mappers;
using Microsoft.EntityFrameworkCore;

//using DataConvert.Models;

namespace WebAppGNAggregator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly ArticleMapper _articleMapper;

        private const int _sourceId = 1;

        private readonly GNAggregatorContext _dbContext;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService, GNAggregatorContext dbContext, ISourceService sourceService, IRssService rssService, ArticleMapper articleMapper)
        {
            _logger = logger;
            _articleService = articleService;
            _dbContext = dbContext;
            _sourceService = sourceService;
            _rssService = rssService;
            _articleMapper = articleMapper;
        }


        [HttpGet]
        public async Task<IActionResult> Index(PaginationModel paginationModel)
        {

            // remove my acc for reg checking
            var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Email.ToLower().Equals("chukhno.d@ya.ru"));
            if (user != null)
            {
                 _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }


            try
            {
                const double minPosRate = -10;
                var articleModels = (await _articleService.GetAllPositiveAsync(minPosRate, paginationModel.PageNumber, paginationModel.PageSize))
                    .Select(article => _articleMapper.ArticleDtoToArticleModel(article))
                    .ToArray();

                var totalArticlesCount = await _articleService.CountAsync(minPosRate);

                _logger.LogInformation("Articles are loaded for page");
                
                var pageInfo = new PageInfo()
                {
                    PageNumber = paginationModel.PageNumber,
                    PageSize = paginationModel.PageSize,
                    TotalItems = totalArticlesCount
                };

                pageInfo.DeviceType = HttpContext.Request.Headers["User-Agent"].ToString().Contains("Mobi") ? 1 : 5;

                ViewBag.PageSize = paginationModel.PageSize;
                ViewBag.Page = paginationModel.PageNumber;

                _logger.LogInformation("Main page /Home/Index was loaded");
                return View(new ArticleModelsCollection
                {
                    ArticleModels = articleModels,
                    PageInfo = pageInfo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null, string? errorMessage = null)
        {
            var errorViewModel = new ErrorViewModel
            {
                StatusCode = statusCode ?? 404,
                ErrorMessage = errorMessage ?? "ѕохоже такой страницы не существует :("
            };

            return View(errorViewModel);
        }

    }
}
