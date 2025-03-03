using GNA.Services.Samples;
using Microsoft.AspNetCore.Mvc;
using WebAppGNAggregator.Models;

namespace WebAppGNAggregator.Controllers
{
    public class SampleController : Controller
    {
        private readonly ITestService _testService;
        private readonly IScopedService _scopedService;
        private readonly ITransientService _transientService;


        public SampleController(ITestService testService, IScopedService scopedService, ITransientService transientService)
        {
            _testService = testService;
            _scopedService = scopedService;
            _transientService = transientService;
        }

        public IActionResult Index()
        {
            var array = new[] { 1, 2, 3, 4, 5, 6, 7 };
            return View(array);
        }


        public IActionResult Test()
        {
            Console.WriteLine("first");
            _transientService.Do();
            _scopedService.Do();
            _testService.Do();
            Console.WriteLine("second");
            _transientService.Do();
            _scopedService.Do();
            _testService.Do();
            return Ok();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }

        public IActionResult Hello(string name)
        {
            ViewData["Name"] = "Bob";
            ViewData["St"] = 123;
            ViewData["Arr"] = new[] { 1, 2, 3, 4, 5 };

            ViewBag.Name = "D";
            ViewBag.Hello = "Hi";
            ViewBag.Smth = new[] { 1, 2, 3, 4, 5 };
            return View();
            //return View(new HelloModel { Name = name });

        }

        [HttpGet]
        public IActionResult Get()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult LoginProcess(LoginModel model)
        {
            return Ok(model);
        }

        public IActionResult THS()
        {

            return View();
        }
    }
}
