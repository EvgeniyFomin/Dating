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
    public class MessagesController(IMessageService messageService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<MessageDto>> AddMessage([FromBody] CreateMessageDto message)
        {
            var currentUserId = User.GetUserId();
            if (currentUserId == message.RecipientId) return BadRequest("You cannot send message to yourself");

            var result = await messageService.AddMessageAsync(currentUserId, message);

            return result == null
                ? BadRequest("Cannot send the message")
                : Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetUsersMessages([FromQuery] MessageParameters parameters)
        {
            parameters.UserId = User.GetUserId();

            var messages = await messageService.GetMessagesForUserAsync(parameters);

            Response.AddPaginationHeader(messages);

            return Ok(messages);
        }

        [HttpGet("thread/{id:int}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetThread([FromRoute] int id)
        {
            var messages = await messageService.GetThreadAsync(User.GetUserId(), id);

            return messages == null
                ? NotFound("There are not messages between these users")
                : Ok(messages);
        }

        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            return await messageService.Delete(messageId, User.GetUserId())
                ? Ok()
                : BadRequest("message was not deleted");
        }
    }
}
