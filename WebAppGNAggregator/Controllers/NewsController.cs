using Microsoft.AspNetCore.Mvc;

namespace WebAppGNAggregator.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
