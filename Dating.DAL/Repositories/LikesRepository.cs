using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
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
        }

        public async Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int userId)
        {
            return await context.Likes
                .Where(x => x.SourceUserId == userId)
                .Select(x => x.TargetUserId)
                .ToListAsync();
        }

        public async Task<UserLike?> GetLikeAsync(int sourceUserId, int targetUserId)
        {
            return await context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<MemberDto>> GetUserLikesAsync(LikesFilteringParameters parameters)
        {
            var likes = context.Likes.AsQueryable();
            IQueryable<User> query;

            switch (parameters.Predicate)
            {
                case "liked":
                    query = likes
                       .Where(x => x.SourceUserId == parameters.UserId)
                       .Select(x => x.TargetUser);
                    break;

                case "likedBy":
                    query = likes
                        .Where(x => x.TargetUserId == parameters.UserId)
                        .Select(x => x.SourceUser);
                    break;

                default:
                    var likeIds = await GetCurrentUserLikeIdsAsync(parameters.UserId);

                    query = likes
                        .Where(x => x.TargetUserId == parameters.UserId && likeIds.Contains(x.SourceUserId))
                        .Select(x => x.SourceUser);
                    break;
            }

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), parameters);
        }

        public void RemoveLike(UserLike userLike)
        {
            context.Likes.Remove(userLike);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
