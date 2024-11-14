using Dating.Core.Models;
using Dating.DAL.Context;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Repositories
{
    public class GroupsRepository(DataContext context) : IGroupsRepository
    {
        public async Task AddGroupAsync(Group group)
        {
            await context.Groups.AddAsync(group);
        }

        public void RemoveConnection(Connection connection)
        {
            context.Connections.Remove(connection);
        }

        public async Task<Connection?> GetConnectionByIdAsync(string connectionId)
        {
            return await context.Connections.FindAsync(connectionId);
        }

        public async Task<Group?> GetGroupByNameAsync(string groupName)
        {
            return await context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<Group?> GetGroupForConnection(string connectionId)
        {
            return await context.Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(y => y.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }
    }
}
