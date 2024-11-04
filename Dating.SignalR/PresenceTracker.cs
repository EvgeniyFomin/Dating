namespace Dating.SignalR
{
    public class PresenceTracker : IPresenceTracker
    {
        private static readonly Dictionary<int, List<string>> _onlineUsers = [];

        public Task UserConnected(int userId, string connectionId)
        {
            lock (_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(userId))
                {
                    _onlineUsers[userId].Add(connectionId);
                }
                else
                {
                    _onlineUsers.Add(userId, [connectionId]);
                }
            }

            return Task.CompletedTask;
        }

        public Task UserDisconnected(int userId, string connectionId)
        {
            lock (_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(userId))
                {
                    _onlineUsers[userId].Remove(connectionId);
                }

                if (_onlineUsers[userId].Count == 0)
                {
                    _onlineUsers.Remove(userId);
                }
            }

            return Task.CompletedTask;
        }

        public Task<int[]> GetOnlineUserIds()
        {
            int[] onlineUsers;
            lock (_onlineUsers)
            {
                onlineUsers = _onlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }
    }
}
