using DataConvert.DTO;
using GNA.Services.Abstractions;
using GNA.Services.Implementations;
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
        private readonly IEmailService _emailService;
        private readonly ICodeGeneratorService _codeGeneratorService;
        private readonly ITotpWithAttemptService _totpWithAttemptService;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger, IEmailService emailService, ICodeGeneratorService codeGeneratorService, ITotpWithAttemptService totpWithAttemptService)
        {
            _accountService = accountService;
            _logger = logger;
            _emailService = emailService;
            _codeGeneratorService = codeGeneratorService;
            _totpWithAttemptService = totpWithAttemptService;
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

                HttpContext.Session.SetString("UserEmail", registerModel.Email);
                _logger.LogInformation($"Mail {registerModel.Email} stored in session");

                var loginDto = await _accountService.TryRegister(registerModel, cancellationToken);
                
                if (loginDto != null)
                {
                    var code = _totpWithAttemptService.GenerateTotpCode(registerModel.Email); 
                    await _emailService.SendEmailAsync(registerModel.Email, code);
                    _logger.LogInformation($"User {registerModel.Email} added but not verified, code sent");
                    return View("Confirm", "");
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

        [HttpPost]
        public IActionResult ConfirmCode(string? code)
        {
            Console.WriteLine($"Confirm code {code} Email:{HttpContext.Session.GetString("UserEmail")}");
            return Content(code??"123");
        }


        [HttpGet]
        public async Task<IActionResult> ResendCode()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Error: Email not found in session.");
                return RedirectToAction("Register");
            }

            var code = _totpWithAttemptService.GenerateTotpCode(email);
            await _emailService.SendEmailAsync(email, code);
            _logger.LogInformation($"New code for {email} created and sent");
            return View("Confirm");
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




        [HttpPost]
        public IActionResult VerifyCode(string inputCode, string token, RegisterModel registerModel)
        {
            if (!_totpWithAttemptService.ValidateToken(token, out int attemptCount, out DateTime lastAttempt))
            {
                return BadRequest("Токен повреждён или истёк.");
            }

            // Ограничение на количество попыток
            if (attemptCount >= 5)
            {
                return BadRequest("Превышено количество попыток.");
            }

            // Ограничение на частоту (например, 1 попытка в минуту)
            if (DateTime.UtcNow < lastAttempt.AddMinutes(1))
            {
                return BadRequest("Попробуйте снова через минуту.");
            }

            // Проверка TOTP
            var generatedTotp = _totpWithAttemptService.GenerateTotpCode(registerModel.Email);
            if (inputCode == generatedTotp)
            {
                return Ok("Код подтверждён успешно!");
            }

            // Генерируем новый токен с увеличенным счётчиком
            var newToken = _totpWithAttemptService.GenerateToken(attemptCount + 1, DateTime.UtcNow);
            return BadRequest(new { Message = "Код неверный.", Token = newToken });
        }

    }
}
