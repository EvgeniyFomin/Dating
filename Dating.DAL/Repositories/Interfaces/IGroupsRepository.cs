using Dating.Core.Models;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface IGroupsRepository
    {
        Task AddGroupAsync(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection?> GetConnectionByIdAsync(string connectionId);
        Task<Group?> GetGroupByNameAsync(string groupName);
        Task<Group?> GetGroupForConnection(string connectionId);
    }
}
