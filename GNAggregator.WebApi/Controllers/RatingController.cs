using EFDatabase.Entities;
using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GNAggregator.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<RatingController> _logger;

        public RatingController(IArticleService articleService, ILogger<RatingController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Rating(CancellationToken cancellationToken = default)
        {
            var isRated = await _articleService.RatingProcess(cancellationToken);
            if (isRated)
            {
                _logger.LogInformation("Articles has been rated");
                return Ok();
            }
            else
            {
                _logger.LogError("Error during rating articles");
                return StatusCode(500);
            }
        }
    }
}
