using Dating.API.Services;
using Dating.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController(IUsersService userService) : ControllerBase
    {
        private readonly IUsersService _usersService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (await _usersService.CheckIfExists(userDto.UserName))
            {
                return BadRequest($"User {userDto.UserName} already exists");
            }

            var user = _usersService.CreateUser(userDto);

            var result = await _usersService.AddAsync(user);

            return result == null
                ? BadRequest("User was not registered")
                : Ok($"User {userDto.UserName} was successfully created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(RegisterUserDto userDto)
        {
            var user = await _usersService.GetByName(userDto.UserName);

            if (user == null)
            {
                return NotFound($"User {userDto.UserName} not foud in the system");
            }

            var res = _usersService.CheckIfPasswordValid(user, userDto.Password);

            return res ? Ok() : Unauthorized("Invalid user or password");
        }
    }
}
