using Dating.API.Extensions;
using Dating.API.Middleware;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController(ILikesService likeService) : ControllerBase
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var sourceUserId = User.GetUserId();

            if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");

            var result = await likeService.LikeToggle(sourceUserId, targetUserId);

            return result ? Ok() : BadRequest("Failed to update like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetUserLikeIds()
        {
            return Ok(await likeService.GetUserLikeIdsAsync(User.GetUserId()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes(string predicate)
        {
            return Ok(await likeService.GetUserLikesAsync(predicate, User.GetUserId()));
        }
    }
}
