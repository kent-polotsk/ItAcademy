using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;

namespace WebAppGNAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger; 

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Login view get was loaded");

            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel,CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                var isLoginOk = await _accountService.TryLogin(loginModel, cancellationToken);

                if (isLoginOk)
                {
                    _logger.LogInformation($"User {loginModel.Email} logged in");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogInformation($"User {loginModel.Email} failed to login");
                    //ViewBag.BadLogin = "Проверьте Email или пароль";
                    ModelState.AddModelError("","Неверный логин или пароль");
                    return View(loginModel);
                    //return View();
                }
            }
            else 
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogOut(LogOutModel loginModel)
        {
            return View();
        }


        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationModel loginModel)
        {
            return View();
        }
        
    }
}
