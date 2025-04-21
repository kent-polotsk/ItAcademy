using Microsoft.AspNetCore.Mvc;

namespace WebAppGNAggregator.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
