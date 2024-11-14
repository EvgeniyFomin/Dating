using AutoMapper;
using Dating.Core.Dtos;
using Dating.Core.Interfaces;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dating.API.Services
{
    public class MessageService(IUnitOfWork unitOfWork, IMapper mapper) : IMessageService
    {
        private readonly IMessagesRepository _messagesRepository = unitOfWork.MessagesRepository;
        private readonly IUsersRepository _usersRepository = unitOfWork.UsersRepository;

        public async Task<MessageDto?> AddMessageAsync(int senderId, CreateMessageDto messageDto)
        {
            var recipient = await _usersRepository.GetByIdAsync(messageDto.RecipientId);
            if (recipient == null || recipient.UserName == null) return null;

            var sender = await _usersRepository.GetByIdAsync(senderId);
            if (sender == null || sender.UserName == null) return null;

            var message = new Message
            {
                SenderId = senderId,
                SenderName = sender.UserName,
                Sender = sender,
                RecipientId = messageDto.RecipientId,
                RecipientName = recipient.UserName,
                Recipient = recipient,
                Content = messageDto.Content
            };

            await _messagesRepository.AddAsync(mapper.Map<Message>(message));

            return await unitOfWork.Complete()
                ? mapper.Map<MessageDto>(message)
                : null;

        }

        public async Task<bool> Delete(int messageId, int userId)
        {
            var message = await _messagesRepository.GetByIdAsync(messageId);
            if (message == null) return false;

            if (message.RecipientId != userId && message.SenderId != userId) return false;

            if (message.SenderId == userId) message.SenderDeleted = true;
            if (message.RecipientId == userId) message.RecipientDeleted = true;

            if (message is { SenderDeleted: true, RecipientDeleted: true }) _messagesRepository.Delete(message);

            return await unitOfWork.Complete();

        }

        public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParameters parameters)
        {
            return await _messagesRepository.GetMessageDtosAsync(parameters);
        }

        public async Task<IEnumerable<MessageDto>> GetThreadAsync(int currentUserId, int recipientId)
        {
            var(query, threads) = await _messagesRepository.GetThreadAsync(currentUserId, recipientId);

            var unreadMessages = query.Where(x => x.ReadDate == null && x.RecipientId == currentUserId);

            if (unreadMessages.Any())
            {
                await unreadMessages.ForEachAsync(x => x.ReadDate = DateTime.UtcNow);
                await unitOfWork.Complete();
            }

            return threads;
        }
    }
}
