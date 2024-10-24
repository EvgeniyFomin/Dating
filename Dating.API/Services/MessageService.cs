using AutoMapper;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
using Dating.DAL.Repositories.Interfaces;

namespace Dating.API.Services
{
    public class MessageService(IMessagesRepository messageRepository, IUsersRepository userRepository, IMapper mapper) : IMessageService
    {
        public async Task<MessageDto?> AddMessageAsync(int senderId, CreateMessageDto messageDto)
        {
            var recipient = await userRepository.GetByIdAsync(messageDto.RecipientId);
            if (recipient == null) return null;

            var sender = await userRepository.GetByIdAsync(senderId);
            if (sender == null) return null;

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

            await messageRepository.AddAsync(mapper.Map<Message>(message));

            return await messageRepository.SaveAllAsync()
                ? mapper.Map<MessageDto>(message)
                : null;

        }

        public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParameters parameters)
        {
            return await messageRepository.GetMessageDtosAsync(parameters);
        }

        public async Task<IEnumerable<MessageDto>> GetThreadAsync(int currentUserId, int recipientId)
        {
            return await messageRepository.GetThreadAsync(currentUserId, recipientId);
        }
    }
}
