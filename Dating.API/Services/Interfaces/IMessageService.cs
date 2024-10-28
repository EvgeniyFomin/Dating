using Dating.Core.Dtos;
using Dating.Core.Models.Pagination;

namespace Dating.API.Services.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto?> AddMessageAsync(int senderId, CreateMessageDto message);
        Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParameters parameters);
        Task<IEnumerable<MessageDto>> GetThreadAsync(int currentUserId, int recipientId);
        Task<bool> Delete(int messageId, int userId);
    }
}
