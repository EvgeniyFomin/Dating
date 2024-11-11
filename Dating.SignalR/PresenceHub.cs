using Microsoft.AspNetCore.SignalR;
using Dating.Core.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Dating.SignalR
{
    [Authorize]
    public class PresenceHub(IPresenceTracker tracker) : Hub
    {
        private const string USER_IS_ONLINE = "UserIsOnline";
        private const string USER_IS_OFFLINE = "UserIsOffline";
        private const string GET_ONLINE_USERS = "GetOnlineUsers";

        public override async Task OnConnectedAsync()
        {
            if (Context.User == null) throw new HubException("Cannot get current user claim from context");

            var isOnline = await tracker.UserConnected(Context.User.GetUserId(), Context.ConnectionId);
            if (isOnline) await Clients.Others.SendAsync(USER_IS_ONLINE, Context.User?.GetUserId());

            var currentUsers = await tracker.GetOnlineUserIds();

            await Clients.Caller.SendAsync(GET_ONLINE_USERS, currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User == null) throw new HubException("Cannot get current user claim from context");

            var isOffline = await tracker.UserDisconnected(Context.User.GetUserId(), Context.ConnectionId);
            if (isOffline) await Clients.Others.SendAsync(USER_IS_OFFLINE, Context.User?.GetUserId());

            await base.OnDisconnectedAsync(exception);
        }
    }
}
