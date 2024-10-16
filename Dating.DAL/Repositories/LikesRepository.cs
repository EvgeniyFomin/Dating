using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.DAL.Context;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Repositories
{
    public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
    {
        public async Task AddLikeAsync(UserLike userLike)
        {
            await context.Likes.AddAsync(userLike);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int currentUserId)
        {
            return await context.Likes
                .Where(x => x.SourceUserId == currentUserId)
                .Select(x => x.TargetUserId)
                .ToListAsync();
        }

        public async Task<UserLike?> GetLikeAsync(int sourceUserId, int targetUserId)
        {
            return await context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<IEnumerable<MemberDto>> GetUserLikesAsync(string predicate, int userId)
        {
            var likes = context.Likes.AsQueryable();

            switch (predicate)
            {
                case "liked":
                    return await likes
                        .Where(x => x.SourceUserId == userId)
                        .Select(x => x.TargetUser)
                        .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                        .ToListAsync();

                case "likedBy":
                    return await likes
                        .Where(x => x.TargetUserId == userId)
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                        .ToListAsync();

                default:
                    var likeIds = await GetCurrentUserLikeIdsAsync(userId);

                    return await likes
                        .Where(x => x.TargetUserId == userId && likeIds.Contains(x.SourceUserId))
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                        .ToListAsync();
            }


        }

        public async Task RemoveLikeAsync(UserLike userLike)
        {
            context.Likes.Remove(userLike);
            await context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
