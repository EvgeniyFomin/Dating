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
        private const string UPDATED_GROUP = "UpdatedGroup";

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["userId"];

            if (Context.User == null || !int.TryParse(otherUser, out int otherUserId)) throw new Exception("Cannot join the group");

            var groupName = GetGroupName(Context.User.GetUserId(), otherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync(UPDATED_GROUP, group);

            var messages = await messageService.GetThreadAsync(Context.User.GetUserId(), otherUserId);

            await Clients.Caller.SendAsync(RECEIVE_MESSAGE_THREAD, messages);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync(UPDATED_GROUP, group);

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

        private async Task<Group> AddToGroup(string groupName)
        {
            var userId = Context.User?.GetUserId() ?? throw new Exception("Cannot get userId from context");
            var group = await messagesRepository.GetGroupByNameAsync(groupName);
            var connection = new Connection { ConnectionId = Context.ConnectionId, UserId = userId };

            group ??= await messagesRepository.AddGroup(new Group { Name = groupName });
            group?.Connections.Add(connection);

            return group ?? throw new HubException("Cannot join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await messagesRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group?.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (connection != null && group != null)
            {
                if (await messagesRepository.RemoveConnectionAsync(connection)) return group;
            }

            throw new Exception("Failed to remove from group");
        }
    }
}
