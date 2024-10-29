﻿using Dating.API.Services.Interfaces;
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

            var (result, user) = await _usersService.CreateUserAsync(registerDto);

            return result.Succeeded
                ? Ok(CreateUserDto(user))
                : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginUserDto registerDto)
        {
            var user = await _usersService.GetByNameAsync(registerDto.UserName);

            if (user == null || user.UserName == null)
            {
                return NotFound($"User {registerDto.UserName} not found in the system");
            }

            var result = await _usersService.CheckIfPasswordValid(user, registerDto.Password);

            if (result) await _usersService.UpdateLastActivityDateAsync(user.Id);

            return result
                ? Ok(await CreateUserDto(user))
                : Unauthorized("Invalid user or password");
        }

        private async Task<UserDto> CreateUserDto(User user)
        {
            return new UserDto
            {
                UserName = user.UserName!,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
                PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url
            };
        }
    }
}