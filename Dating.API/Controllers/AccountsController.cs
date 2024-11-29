using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController(IAccountService accountService, ITokenService tokenService) : ControllerBase
    {
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto registerDto)
        {
            if (await accountService.CheckIfExistsAsync(registerDto.UserName))
            {
                return BadRequest($"User {registerDto.UserName} already exists");
            }

            var (result, user) = await accountService.CreateAccountAsync(registerDto);

            return result.Succeeded
                ? Ok(await CreateUserDto(user))
                : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto registerDto)
        {
            var user = await accountService.GetByNameAsync(registerDto.UserName);

            if (user == null || user.UserName == null)
            {
                return Unauthorized($"User {registerDto.UserName} not found in the system");
            }

            var result = await accountService.CheckIfPasswordValid(user, registerDto.Password);

            if (result) await accountService.UpdateLastActivityDateAsync(user.Id);

            return result
                ? Ok(await CreateUserDto(user))
                : Unauthorized("Invalid user or password");
        }

        private async Task<UserDto> CreateUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
                PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url
            };
        }
    }
}