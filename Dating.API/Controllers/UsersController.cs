using Dating.API.Extensions;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUsersService usersService, IPhotoService photoService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;
        private readonly IPhotoService _photoService = photoService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
        {
            var resultsDto = await _usersService.GetAllMemberDtosAsync();

            return resultsDto.Any()
                ? Ok(resultsDto)
                : NotFound("No users are regiristered in the system yet");
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<ActionResult<MemberDto>> GetById([FromRoute] int id)
        {
            var resultDto = await _usersService.GetMemberDtoByIdAsync(id);

            return resultDto == null
                ? NotFound($"No user found with ID: {id}")
                : Ok(resultDto);
        }

        [Route("{username}")]
        [HttpGet]
        public async Task<ActionResult<MemberDto>> GetByUsername([FromRoute] string userName)
        {
            var resultDto = await _usersService.GetMemberDtoByNameAsync(userName);

            return resultDto == null
                ? NotFound($"No user found with name: {userName}")
                : Ok(resultDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MemberUpdateDto updateDto)
        {
            return (await _usersService.UpdateUserAsync(updateDto, User.GetUsername()))
                ? NoContent()
                : BadRequest("User was not updated");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _usersService.GetByNameAsync(User.GetUsername());
            if (user == null) return BadRequest("User cannot be updated");

            var photo = await _photoService.AddPhotoAsync(file);
            if (photo == null) return BadRequest("Photo was not uploaded");

            if (!await _usersService.AddPhotoToUserAsync(user, photo))
                return BadRequest("User was not updated - photo not added");

            return CreatedAtAction(
                    nameof(GetByUsername),
                    new { username = user.UserName },
                    _photoService.MapToDto(photo));
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _usersService.GetByNameAsync(User.GetUsername());
            if (user == null) return BadRequest("User cannot be found");

            return (await _usersService.SetPhotoAsMainToUserAsync(user, photoId))
                    ? NoContent()
                    : BadRequest("User's main photo was not updated");
        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _usersService.GetByNameAsync(User.GetUsername());
            if (user == null) return BadRequest("User cannot be found");

            var publicId = await _usersService.DeletePhotoReturnPublicIdAsync(user, photoId);

            if (string.IsNullOrWhiteSpace(publicId)) return BadRequest("User's photo was not deleted");

            var deletionResult = await _photoService.DeletePhotoAsync(publicId);

            return deletionResult.Error == null
                ? NoContent()
                : throw new Exception($"Photo with PublicId: {publicId}");
        }
    }
}
