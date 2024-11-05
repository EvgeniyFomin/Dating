using Dating.Core.Dtos;
using Dating.Core.Extensions;
using Dating.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Dating.SignalR
{
    public class MessageHub(IMessageService messageService) : Hub
    {
        private const string RECEIVE_MESSAGE_THREAD = "ReceiveMessageThread";
        private const string NEW_MESSAGE = "NewMessage";

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["userId"];

            if (Context.User == null || !int.TryParse(otherUser, out int otherUserId)) throw new Exception("Cannot join the group");

            var groupName = GetGroupName(Context.User.GetUserId(), otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await messageService.GetThreadAsync(Context.User.GetUserId(), otherUserId);

            await Clients.Group(groupName).SendAsync(RECEIVE_MESSAGE_THREAD, messages);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto messageDto)
        {
            var addedMessageDto = await messageService.AddMessageAsync(Context.User!.GetUserId(), messageDto);

            if (addedMessageDto == null)
            {
                throw new HubException("Massage was not sent");
            }
            else
            {
                var group = GetGroupName(addedMessageDto.Sender.Id, addedMessageDto.Recipient.Id);
                await Clients.Group(group).SendAsync(NEW_MESSAGE, addedMessageDto);
            }
        }

        private static string GetGroupName(int callerId, int otherUserId)
        {
            return callerId < otherUserId
                ? $"{callerId}-{otherUserId}"
                : $"{otherUserId}-{callerId}";
        }
    }
}
