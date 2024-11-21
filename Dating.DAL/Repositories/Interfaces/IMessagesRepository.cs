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
        Task<(IQueryable<Message> query, IEnumerable<MessageDto> messageDtos)> GetThreadAsync(int currentUserId, int recipientId);
        Task UpdateReadDate(MessageDto messageDto);
    }
}
