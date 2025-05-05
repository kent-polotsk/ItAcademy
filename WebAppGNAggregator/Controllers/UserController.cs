using DAL_CQS_.Queries;
using DataConvert.DTO;
using EFDatabase.Entities;
using Mappers.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Profile(UserDto userDto = null)
        {
            //var isSaved = _userService.SaveUser(userDto);
            
            return View("Profile", new[] { -4.3, 3.3 });
        }
    }
}
