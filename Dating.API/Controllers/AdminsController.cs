using Dating.API.Services.Interfaces;
using Dating.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dating.API.Controllers
{
    [Route("api/[controller]")]
    public class AdminsController(UserManager<User> userManager, IPhotoService photoService) : ControllerBase
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await userManager.Users
                .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    x.Id,
                    UserName = x.UserName,
                    Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
                }).ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-user-roles/{userId}")]
        public async Task<ActionResult> EditRoles(string userId, string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null) return BadRequest("User not found");

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-for-approval")]
        public async Task<ActionResult> GetPhotosForApproval()
        {
            var result = await photoService.GetUnapprovedPhotoDtosAsync();

            return Ok(result);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPut("approve-photo/{id:int}")]
        public async Task<ActionResult> ApprovePhoto(int id)
        {
            return await photoService.ApprovePhotoAsync(id)
                 ? Ok()
                 : BadRequest("Photo was not approved");
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpDelete("reject-photo/{id:int}")]
        public async Task<ActionResult> RejectPhoto(int id)
        {
            try
            {
                return await photoService.RemovePhotoAsync(id)
                                 ? Ok()
                                 : BadRequest("Photo was not approved");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
