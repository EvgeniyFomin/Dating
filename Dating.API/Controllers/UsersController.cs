using Dating.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resultsDto = await _usersService.GetAllMemberDtosAsync();

            return resultsDto.Any()
                ? Ok(resultsDto)
                : NotFound("No users are regiristered in the system yet");
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var resultDto = await _usersService.GetMemberDtoByIdAsync(id);

            return resultDto == null
                ? NotFound($"No user found with ID: {id}")
                : Ok(resultDto);
        }

        [Route("{username}")]
        [HttpGet]
        public async Task<IActionResult> GetByUsername([FromRoute] string userName)
        {
            var resultDto = await _usersService.GetMemberDtoByNameAsync(userName);

            return resultDto == null
                ? NotFound($"No user found with name: {userName}")
                : Ok(resultDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var result = await _usersService.DeleteByIdAsync(id);

            return result
                ? Ok()
                : BadRequest();
        }
    }
}
