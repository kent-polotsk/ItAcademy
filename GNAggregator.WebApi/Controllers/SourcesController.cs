using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GNAggregator.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourcesController : ControllerBase
    {
        private readonly ISourceService _sourceService;
        private readonly ILogger<SourcesController> _logger;

        public SourcesController(ISourceService sourceService, ILogger<SourcesController> logger)
        {
            _sourceService = sourceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles(CancellationToken cancellationToken = default)
        {
            var sources = await _sourceService.GetSourceDtosWithRssAsync(cancellationToken);
            if (sources != null)
            {
                _logger.LogInformation($"Sources with RSS successfully loaded");
                return Ok(sources);
            }
            return BadRequest();
        }

    }
}
