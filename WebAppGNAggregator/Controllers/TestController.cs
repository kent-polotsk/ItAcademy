using Microsoft.AspNetCore.Mvc;

namespace WebAppGNAggregator.Controllers
{
    public class TestController : Controller
    {
        public IActionResult T1(string car)
        {
            return Ok(string.IsNullOrEmpty(car) ? "123" : car);
        }
        public string T2(string car)
        {
            return string.IsNullOrEmpty(car) ? "boomer" : car;

        }
    }
}
