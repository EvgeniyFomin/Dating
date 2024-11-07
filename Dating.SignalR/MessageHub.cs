using Dating.Core.Dtos;
using Dating.Core.Extensions;
using Dating.Core.Interfaces;
using Dating.Core.Models;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Dating.SignalR
{
    public class MessageHub(
        IMessageService messageService,
        IMessagesRepository messagesRepository,
        IHubContext<PresenceHub> presenceHub,
        IPresenceTracker presenceTracker) : Hub
    {
        private const string RECEIVE_MESSAGE_THREAD = "ReceiveMessageThread";
        private const string NEW_MESSAGE = "NewMessage";
        private const string NEW_MESSAGE_RECEIVED = "NewMessageReceived";

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["userId"];

            if (Context.User == null || !int.TryParse(otherUser, out int otherUserId)) throw new Exception("Cannot join the group");

            var groupName = GetGroupName(Context.User.GetUserId(), otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await AddToGroup(groupName);

            var messages = await messageService.GetThreadAsync(Context.User.GetUserId(), otherUserId);

            await Clients.Group(groupName).SendAsync(RECEIVE_MESSAGE_THREAD, messages);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await RemoveFromMessageGroup();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto messageDto)
        {
            var addedMessageDto = await messageService.AddMessageAsync(Context.User!.GetUserId(), messageDto)
                ?? throw new HubException("Massage was not sent");

            var groupName = GetGroupName(addedMessageDto.Sender.Id, addedMessageDto.Recipient.Id);
            var group = await messagesRepository.GetGroupByNameAsync(groupName);

            if (group != null && group.Connections.Any(x => x.UserId == addedMessageDto.Recipient.Id))
            {
                addedMessageDto.ReadDate = DateTime.UtcNow;
                await messagesRepository.UpdateReadDate(addedMessageDto);
            }
            else
            {
                var connections = await presenceTracker.GetConnectionsForUser(addedMessageDto.Recipient.Id);
                if (connections != null && connections.Count != 0)
                {
                    await presenceHub.Clients.Clients(connections)
                        .SendAsync(NEW_MESSAGE_RECEIVED, new
                        {
                            userId = addedMessageDto.Sender.Id,
                            knownAs = addedMessageDto.Sender.KnownAs
                        });
                }
            }

            await Clients.Group(groupName).SendAsync(NEW_MESSAGE, addedMessageDto);
        }

        private static string GetGroupName(int callerId, int otherUserId)
        {
            return callerId < otherUserId
                ? $"{callerId}-{otherUserId}"
                : $"{otherUserId}-{callerId}";
        }

        private async Task<bool> AddToGroup(string groupName)
        {
            var userId = Context.User?.GetUserId() ?? throw new Exception("Cannot get userId from context");
            var group = await messagesRepository.GetGroupByNameAsync(groupName);
            var connection = new Connection { ConnectionId = Context.ConnectionId, UserId = userId };

            if (group == null)
            {
                group = new Group { Name = groupName };
                await messagesRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            return await messagesRepository.SaveAllAsync();
        }

        private async Task RemoveFromMessageGroup()
        {
            var connection = await messagesRepository.GetConnectionByIdAsync(Context.ConnectionId);
            if (connection != null)
            {
                messagesRepository.RemoveConnection(connection);
                await messagesRepository.SaveAllAsync();
            }
        }
    }
}
