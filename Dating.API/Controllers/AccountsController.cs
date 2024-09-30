using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController(
        IUsersService usersService,
        ITokenService tokenService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto registerDto)
        {
            if (await _usersService.CheckIfExistsAsync(registerDto.UserName))
            {
                return BadRequest($"User {registerDto.UserName} already exists");
            }

            var user = await _usersService.CreateUserAsync(registerDto);

            return user == null
                ? BadRequest("User was not registered")
                : Ok(new UserDto
                {
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(RegisterUserDto registerDto)
        {
            var user = await _usersService.GetByNameAsync(registerDto.UserName);

            if (user == null)
            {
                return NotFound($"User {registerDto.UserName} not foud in the system");
            }

            var result = _usersService.CheckIfPasswordValid(user, registerDto.Password);

            return result
                ? Ok(
                    new UserDto
                    {
                        UserName = user.UserName,
                        Token = _tokenService.CreateToken(user)
                    })
                : Unauthorized("Invalid user or password");
        }
    }
}
