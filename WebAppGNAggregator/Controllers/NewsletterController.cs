using EFDatabase;
using GNA.Services.Abstractions;
using Mappers.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppGNAggregator.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NewsletterController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly INewsletterService _newsletterService;



        public NewsletterController(ILogger<HomeController> logger, INewsletterService newsletterService)
        {
            _logger = logger;
            _newsletterService = newsletterService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendNewsletters()
        {
            try
            {
                await _newsletterService.SendEmailsAsync();
                _logger.LogInformation("Sending successfully");
                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                //HttpContext.Response.StatusCode = 404;
                return RedirectToAction("Error", "Home", new
                {
                    statusCode = 500,
                    errorMessage = $"Непредвиденная ошибка при рассылке :(<br>{ex.Message}"
                });
            }
            return View();
        }
    }
}
