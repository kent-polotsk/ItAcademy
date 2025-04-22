using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using System.Threading;

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
                    ModelState.AddModelError("","Неверный логин или пароль");
                    return View(loginModel);
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


        // 12345aA!

        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation($"Show register view");
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"User {registerModel.Email} checking ");
            
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"User {registerModel.Email} model is valid");
                var success = await _accountService.TryRegister(registerModel, cancellationToken);

                _logger.LogInformation($"User {registerModel.Email} registered");
                if (success)
                {
                    _logger.LogInformation($"User {registerModel.Email} registered");
                    TempData["ToastMessage"] = "Вы зарегистрированы :)";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogInformation($"User {registerModel.Email} already exists");
                    ModelState.AddModelError("", "Такой пользователь уже существует");
                    return View(registerModel);
                }
            }
            else
            {
                return View(registerModel);
            }
        }
        
    }
}
