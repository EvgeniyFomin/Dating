using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface IMessagesRepository
    {
        Task AddAsync(Message message);
        void Delete(Message message);
        Task<Message?> GetByIdAsync(int id);
        Task<PagedList<MessageDto>> GetMessageDtosAsync(MessageParameters parameters);
        Task<IEnumerable<MessageDto>> GetThreadAsync(int currentUserId, int recipientId);
        Task UpdateReadDate(MessageDto messageDto);
        Task<bool> SaveAllAsync();

        // TODO think of interface segregation
        Task AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection?> GetConnectionByIdAsync(string connectionId);
        Task<Group?> GetGroupByNameAsync(string groupName);
    }
}
