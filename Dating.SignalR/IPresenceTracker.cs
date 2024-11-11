namespace Dating.SignalR
{
    public interface IPresenceTracker
    {
        Task<bool> UserConnected(int userId, string connectionId);
        Task<bool> UserDisconnected(int userId, string connectionId);
        Task<int[]> GetOnlineUserIds();
        Task<List<string>> GetConnectionsForUser(int userId);
    }
}
