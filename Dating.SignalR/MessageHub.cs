using Dating.Core.Extensions;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Dating.SignalR
{
    public class MessageHub(IMessagesRepository messagesRepository) : Hub
    {
        private const string RECEIVE_MESSAGE_THREAD = "ReceiveMessageThread";

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["userId"];

            if (Context.User == null || int.TryParse(otherUser, out int otherUserId)) throw new Exception("Cannot join the group");

            var groupName = GetGroupName(Context.User.GetUserId(), otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await messagesRepository.GetThreadAsync(Context.User.GetUserId(), otherUserId);

            await Clients.Group(groupName).SendAsync(RECEIVE_MESSAGE_THREAD, messages);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        private static string GetGroupName(int callerId, int otherUserId)
        {
            return callerId < otherUserId
                ? $"{callerId}-{otherUserId}"
                : $"{otherUserId}-{callerId}";
        }
    }
}
