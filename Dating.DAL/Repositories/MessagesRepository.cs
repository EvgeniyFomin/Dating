using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dating.Core.Dtos;
using Dating.Core.Enums;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
using Dating.DAL.Context;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Repositories
{
    public class MessagesRepository(DataContext context, IMapper mapper) : IMessagesRepository
    {
        public async Task AddAsync(Message message)
        {
            await context.Messages.AddAsync(message);
        }

        public void Delete(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Message?> GetByIdAsync(int id)
        {
            return await context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessageDtosAsync(MessageParameters parameters)
        {
            var query = context.Messages
                .OrderByDescending(m => m.SentDate)
                .AsQueryable();

            query = parameters.Container switch
            {
                Container.Inbox => query.Where(m => m.RecipientId == parameters.UserId && m.RecipientDeleted == false),
                Container.Outbox => query.Where(m => m.SenderId == parameters.UserId && m.SenderDeleted == false),
                _ => query.Where(m => m.RecipientId == parameters.UserId && m.ReadDate == null && m.RecipientDeleted == false)
            };

            return await PagedList<MessageDto>.CreateAsync(
                query.ProjectTo<MessageDto>(mapper.ConfigurationProvider),
                parameters);
        }

        public async Task<IEnumerable<MessageDto>> GetThreadAsync(int currentUserId, int recipientId)
        {
            var query = context.Messages
                .Where(x =>
                    x.SenderId == currentUserId
                        && x.SenderDeleted == false
                        && x.RecipientId == recipientId ||
                    x.SenderId == recipientId
                        && x.RecipientDeleted == false
                        && x.RecipientId == currentUserId
                )
                .OrderBy(x => x.SentDate);

            await MarkAllUnreadAsRead(query, currentUserId);

            return await query.ProjectTo<MessageDto>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        private async Task MarkAllUnreadAsRead(IQueryable<Message> query, int currentUserId)
        {
            var unreadMessages = query.Where(x => x.ReadDate == null && x.RecipientId == currentUserId);

            if (unreadMessages.Any())
            {
                await unreadMessages.ForEachAsync(x => x.ReadDate = DateTime.UtcNow);
                _ = await SaveAllAsync();
            }
        }
    }
}
