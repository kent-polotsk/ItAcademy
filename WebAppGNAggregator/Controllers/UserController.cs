using DAL_CQS_.Commands;
using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase.Entities;
using Mappers.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;

namespace WebAppGNAggregator.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserMapper _userMapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, UserMapper userMapper, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _userMapper = userMapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userEmail = User.Claims.FirstOrDefault().Value;

            if (userEmail != null)
            {
                var user = await _mediator.Send(new CheckUserEmailExistsQuery() { Email = userEmail });
                if (user != null)
                {
                    var userDto = _userMapper.UserToUserDto(user);
                    _logger.LogInformation($"User {userDto.Email} was found ");
                    return View(userDto);
                }
                else
                {
                    _logger.LogWarning("User not found "+userEmail);
                    return View("Index", "Home");
                }
            }
            else
            {
                _logger.LogWarning("User not found " + userEmail);
                return View("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Profile(UserDto userDto = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isSaved = false;
                    isSaved = await _mediator.Send(new UpdateUserByIdCommand() { userDto = userDto });
                    if (isSaved)
                    {
                        _logger.LogInformation($"Profile of user {userDto.Email} sussessfully saved");
                        TempData["Saved"] = "Данные сохранены";
                        return View(userDto);
                    }
                    else
                    {
                        _logger.LogWarning($"User {userDto.Email} not found");
                        return RedirectToAction("Error", "Home", new { statusCode = 404, errorMessage = "Похоже такого пользователя нет :(<br>" });
                        throw new Exception();
                    }
                }
                else
                {
                    return View(userDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"User {userDto.Email} not found");
                return RedirectToAction("Error", "Home", new { statusCode = 404, errorMessage = "Похоже такого пользователя нет :(<br>" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid? id)
        {
            try
            {
                bool isDeleted = false;
                if (id != null)
                {
                    isDeleted = await _mediator.Send(new DeleteUserByIdCommand() { Id = (Guid)id });
                }

                if (isDeleted == true)
                {
                    _logger.LogInformation($"User Id:{id} deleted successfully (UserController)");
                    return RedirectToAction("LogOut", "Account");
                }
                else
                {
                    _logger.LogWarning($"User Id:{id} not found");
                    return RedirectToAction("Error", "Home", new { statusCode = 404, errorMessage = "Не удается найти пользователя для удаления :(<br>" });
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 404;
                return RedirectToAction("Error", "Home", new
                {
                    statusCode = 404,
                    errorMessage = "Данные по пользователю не найдены :(<br>"
                });
            }
        }

    }
}
