using Dating.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _usersService.GetAllAsync();

            return Ok(result);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _usersService.GetByIdAsync(id);

            return result == null ? NotFound($"No user found with ID: {id}") : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var result = await _usersService.DeleteByIdAsync(id);

            return result ? Ok() : BadRequest();
        }
    }
}
