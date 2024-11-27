using Dating.API.Middleware;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Extensions;
using Dating.Core.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUsersService usersService, IPhotoService photoService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserFilteringParameters parameters)
        {
            parameters.CurrentUserName = User.GetUserName();
            var resultDto = await usersService.GetPagedMemberDtosAsync(parameters);

            Response.AddPaginationHeader(resultDto);

            return resultDto.Any()
                ? Ok(resultDto)
                : NotFound("No users are regiristered in the system yet");
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<ActionResult<MemberDto>> GetById([FromRoute] int id)
        {
            var resultDto = await usersService.GetMemberDtoByIdAsync(id, id == User.GetUserId());

            return resultDto == null
                ? NotFound($"No user found with ID: {id}")
                : Ok(resultDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MemberUpdateDto updateDto)
        {
            return (await usersService.UpdateUserAsync(updateDto, User.GetUserName()))
                ? NoContent()
                : BadRequest("User was not updated");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await usersService.GetByIdAsync(User.GetUserId(), true);
            if (user == null) return BadRequest("User cannot be updated");

            var photo = await photoService.AddPhotoAsync(file, user);

            return CreatedAtAction(
                    nameof(GetById),
                    new { id = user.Id },
                    photoService.MapToDto(photo));
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await usersService.GetByIdAsync(User.GetUserId(), true);
            if (user == null) return BadRequest("User cannot be found");

            return (await usersService.SetPhotoAsMainToUserAsync(user, photoId))
                    ? NoContent()
                    : BadRequest("User's main photo was not updated");
        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await usersService.GetByIdAsync(User.GetUserId(), true);
            if (user == null) return BadRequest("User cannot be found");

            return await photoService.DeletePhotoAsync(photoId, user)
                ? NoContent()
                : BadRequest("photo was not deleted");
        }
    }
}