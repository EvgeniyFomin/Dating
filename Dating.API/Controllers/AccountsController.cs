using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
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

            return user == null || user.UserName == null
                ? BadRequest("User was not registered")
                : Ok(CreateUserDto(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto registerDto)
        {
            var user = await _usersService.GetByNameAsync(registerDto.UserName);

            if (user == null || user.UserName == null)
            {
                return NotFound($"User {registerDto.UserName} not foud in the system");
            }

            var result = _usersService.CheckIfPasswordValid(user, registerDto.Password);

            if (result) await _usersService.UpdateLastActivityDateAsync(user.Id);

            return result
                ? Ok(CreateUserDto(user))
                : Unauthorized("Invalid user or password");
        }

        private UserDto CreateUserDto(User user)
        {
            return new UserDto
            {
                UserName = user.UserName!,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
                PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url
            };
        }
    }
}