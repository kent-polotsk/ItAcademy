using DAL_CQS_.Commands;
using DataConvert.DTO;
using GNA.Services.Abstractions;
using Jose;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using System.Security.Claims;
using System.Text;

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
                HttpContext.Session.SetString("Email", registerModel.Email);
                var loginDto = await _accountService.TryRegister(registerModel, cancellationToken);
                _logger.LogInformation($"Start register new user {registerModel.Email}");

                if (loginDto != null)
                {
                    var code = _totpCodeService.GenerateTotpCode(registerModel.Email);
                    await _emailService.SendEmailAsync(registerModel.Email, code);
                    var token = _accountService.GenerateSecureToken(registerModel.Email, 0);
                    var encryptedToken = _accountService.EncryptToken(token);

                    _logger.LogInformation($"Confirmation code sent to {registerModel.Email}"); 
                    HttpContext.Session.SetString("Token", encryptedToken); 
                    
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
            var email = HttpContext.Session.GetString("Email");
            if (encryptedToken != null && email != null)
            {
                if (!string.IsNullOrEmpty(code) && code != null)
                {
                    var result = await _accountService.ValidateSecureTokenAsync(HttpContext.Session, code);
                    if (result.LoginDto != null)
                    {

                        HttpContext.Session.Remove("Token");
                        
                        var isVerified = await _mediator.Send(new VerifyConfirmedUserCommand() { Email = email });// verify user in db
                       
                        if (!isVerified) 
                        {
                            _logger.LogError($"Error during verifying {email} in DB");
                            await _mediator.Send(new DeleteNotConfirmedUserCommand() { Email = email }); //delete if not confirmed
                            return RedirectToAction("Error", "Home", new { statusCode = 405, errorMessage = "Неверный код (null or empty). Регистрация не завершена." });
                        }

                        await SignIn(result.LoginDto);
                        _logger.LogInformation($"User {email} has confirm 2FA code and signed in");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        
                        _logger.LogWarning($"User {email} doesn't confirm code : entered code doesn't match");
                         await _mediator.Send(new DeleteNotConfirmedUserCommand() { Email = email }); //delete if not confirmed
                        return RedirectToAction("Error", "Home", new { statusCode = 401, errorMessage = "Неверный код. Регистрация не завершена." });
                    }
                }
                else
                {
                    _logger.LogError($"User {email} doesn't confirm code : code is null");
                     await _mediator.Send(new DeleteNotConfirmedUserCommand() { Email = email }); //delete if not confirmed
                    return RedirectToAction("Error", "Home", new { statusCode = 500, errorMessage = "Неверный код. Регистрация не завершена." });
                }
            }
            else
            {
                _logger.LogError($"User {email} doesn't confirm code : token is null");
                await _mediator.Send(new DeleteNotConfirmedUserCommand() { Email = email }); //delete if not confirmed
                return RedirectToAction("Error", "Home", new { statusCode = 500, errorMessage = "Неверный токен. Регистрация не завершена." });
            }
            _logger.LogError($"User {email} doesn't confirm token : token is null");
            var isDeleted = await _mediator.Send(new DeleteNotConfirmedUserCommand() { Email = email }); //delete if not confirmed
            return RedirectToAction("Error", "Home", new { statusCode = 405, errorMessage = "Неверный код (null or empty). Регистрация не завершена." });
        }


        [HttpGet]
        public async Task<IActionResult> ResendCode()
        {
            var email = HttpContext.Session.GetString("Email");
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                _logger.LogError($"Error: Email or token not found in session.");
                return RedirectToAction("Register");
            }

            var result = await _accountService.ValidateSecureTokenAsync(HttpContext.Session,"");

            if (result.Attempts <= 4)
            {
                var code = _totpCodeService.GenerateTotpCode(email);
                await _emailService.SendEmailAsync(email, code);
                _logger.LogInformation($"New code for {email} created and sent, token updated");
            }
            else
            {
                _logger.LogInformation($"{email} not confirmed: attempts limit");
                await _mediator.Send(new DeleteNotConfirmedUserCommand() { Email = email});
            }
            TempData["Attempts"] = 4 - result.Attempts;
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
