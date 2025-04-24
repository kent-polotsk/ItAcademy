using DataConvert.DTO;
using GNA.Services.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using System.Security.Claims;
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
        public async Task<IActionResult> Login(LoginModel loginModel, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {

                var loginDto = await _accountService.TryLogin(loginModel, cancellationToken);

                if (loginDto != null)
                {
                    await SignIn(loginDto);

                    _logger.LogInformation($"User {loginModel.Email} logged in");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogInformation($"User {loginModel.Email} failed to login");
                    ModelState.AddModelError("", "Неверный логин или пароль");
                    return View(loginModel);
                }
            }
            else
            {
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> LogOut()
        {

            var userName = User.Identity?.Name;
            _logger.LogInformation($"User {userName} was logged out");
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // 12345aA!

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"User {registerModel.Email} model is valid");
                var loginDto = await _accountService.TryRegister(registerModel, cancellationToken);

                if (loginDto != null)
                {
                    _logger.LogInformation($"User {registerModel.Email} registered");
                    TempData["ToastMessage"] = "Вы зарегистрированы :)";

                    await SignIn(loginDto);
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



        private async Task SignIn(LoginDto loginDto)
        {
            var claims = new List<Claim>
                    {
                        new(ClaimTypes.Email,loginDto.Email),
                        new(ClaimTypes.Role,loginDto.Role),
                        new(ClaimTypes.Name,loginDto.Email)
                    };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation($"Claims are created for {claims[0].Value}");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

    }
}
