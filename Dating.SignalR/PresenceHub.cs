using Microsoft.AspNetCore.SignalR;
using Dating.Core.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Dating.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private const string USER_IS_ONLINE = "UserIsOnline";
        private const string USER_IS_OFFLINE = "UserIsOffline";

        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync(USER_IS_ONLINE, Context.User.GetUserName());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync(USER_IS_OFFLINE, Context.User.GetUserName());
            await base.OnDisconnectedAsync(exception);
        }
    }
}
