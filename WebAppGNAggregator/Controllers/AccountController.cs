using DAL_CQS_.Queries;
using DataConvert.DTO;
using GNA.Services.Abstractions;
using GNA.Services.Implementations;
using MediatR;
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
        private readonly ITotpCodeService _totpCodeService;
        private readonly IMediator _mediator;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger, IEmailService emailService, ICodeGeneratorService codeGeneratorService, ITotpCodeService totpCodeService, IMediator mediator)
        {
            _accountService = accountService;
            _logger = logger;
            _emailService = emailService;
            _codeGeneratorService = codeGeneratorService;
            _totpCodeService = totpCodeService;
            _mediator = mediator;
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
                //_logger.LogInformation($"User {registerModel.Email} model is valid");

                HttpContext.Session.SetString("UserEmail", registerModel.Email);
                //_logger.LogInformation($"Mail {registerModel.Email} stored in session"); //tmp

                var loginDto = await _accountService.TryRegister(registerModel, cancellationToken);
                
                if (loginDto != null)
                {
                    var code = _totpCodeService.GenerateTotpCode(registerModel.Email);
                    //Console.WriteLine($"code from acc controll {code} before sent"); //tmp
                    await _emailService.SendEmailAsync(registerModel.Email, code);
                    //_logger.LogInformation($"User {registerModel.Email} added but not verified, code sent");

                    var token = _accountService.GenerateSecureToken(registerModel.Email);
                    var encryptedToken = _accountService.EncryptToken(token);
                    HttpContext.Session.SetString("Token",encryptedToken);  //save token to session
                    _logger.LogInformation($"Token {token}");   //tmp log

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
        public async Task<IActionResult> ConfirmCode(string? code)
        {
            var encryptedToken = HttpContext.Session.GetString("Token");
            if (encryptedToken != null)
            {
                var token = _accountService.DecryptToken(encryptedToken);


                var loginDto = await _accountService.ValidateSecureTokenAsync(token, code);
                if (loginDto!=null)
                {
                    //HttpContext.Session.Remove("Token");
                    await SignIn(loginDto);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    HttpContext.Session.Remove("Token");


                   // Console.WriteLine($"Confirm code {code} Email:{HttpContext.Session.GetString("UserEmail")}");
                }
            }
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

            var code = _totpCodeService.GenerateTotpCode(email);
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

    }
}
