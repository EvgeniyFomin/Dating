namespace Dating.SignalR
{
    public class PresenceTracker : IPresenceTracker
    {
        private static readonly Dictionary<int, List<string>> _onlineUsers = [];

        public Task<bool> UserConnected(int userId, string connectionId)
        {
            var isOnline = false;

            lock (_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(userId))
                {
                    _onlineUsers[userId].Add(connectionId);
                }
                else
                {
                    _onlineUsers.Add(userId, [connectionId]);
                    isOnline = true;
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(int userId, string connectionId)
        {
            var isOfline = false;
            lock (_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(userId))
                {
                    _onlineUsers[userId].Remove(connectionId);
                }

                if (_onlineUsers[userId].Count == 0)
                {
                    _onlineUsers.Remove(userId);
                    isOfline = true;
                }
            }

            return Task.FromResult(isOfline);
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

        public async Task<List<string>> GetConnectionsForUser(int userId)
        {
            List<string> connectionIds = [];

            if (_onlineUsers.TryGetValue(userId, out var connections))
            {
                lock (connections)
                {
                    connectionIds = connections;
                }
            }
            else
            {
                connectionIds = [];
            }

            return await Task.FromResult(connectionIds);
        }
    }
}
