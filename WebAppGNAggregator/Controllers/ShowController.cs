using Microsoft.AspNetCore.Mvc;
using WebAppGNAggregator.Models;
//using WebAppGNAggregator.Views;

namespace WebAppGNAggregator.Controllers
{
    
    public class ShowController : Controller
    {
        public IActionResult InfoT1(string car)
        {
            return Ok(string.IsNullOrEmpty(car) ? "123" : car);
        }
        public string InfoT2(string car)
        {
            return string.IsNullOrEmpty(car) ? "boomer" : car;

        }


        
        public IActionResult ShowText() 
        {
            return View(new ShowTextModel(){text = "12345678" });
        }

    }
}
