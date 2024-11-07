namespace Dating.SignalR
{
    public interface IPresenceTracker
    {
        Task UserConnected(int userId, string connectionId);
        Task UserDisconnected(int userId, string connectionId);
        Task<int[]> GetOnlineUserIds();
        Task<List<string>> GetConnectionsForUser(int userId);
    }
}
