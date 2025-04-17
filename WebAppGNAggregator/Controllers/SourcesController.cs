using EFDatabase;
using GNA.Services.Abstractions;
using GNA.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using DataConvert.Models;

namespace WebAppGNAggregator.Controllers
{
    public class SourcesController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly ILogger<HomeController> _logger;

        public SourcesController(ILogger<HomeController> logger,ISourceService sourceService)
        {
            _logger = logger;
            _sourceService = sourceService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            try
            {
                var sources = await _sourceService.GetSourceWithRssAsync(cancellationToken);

                if (sources == null || sources.Length == 0)
                {
                    _logger.LogError("There are no sources with RSS");
                     throw new ArgumentNullException("Источники с RSS не найдены")  ;
                }
                else
                {
                    _logger.LogInformation("Sources with RSS successfully loaded");
                    return View(sources);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading sources");
                HttpContext.Response.StatusCode = 404;
                ViewBag.ErrorMessage = "Источники не найдены";
                throw;
            }
        }
    }
}
