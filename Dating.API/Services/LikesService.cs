using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.DAL.Repositories.Interfaces;

namespace Dating.API.Services
{
    public class LikesService(ILikesRepository repository) : ILikesService
    {
        public async Task<bool> LikeToggle(int sourceUserId, int targetUserId)
        {
            var existingLike = await repository.GetLikeAsync(sourceUserId, targetUserId);

            if (existingLike == null)
            {
                var like = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };

                await repository.AddLikeAsync(like);
            }
            else
            {
                repository.RemoveLike(existingLike);
            }

            return await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> GetUserLikeIdsAsync(int currentUserId)
        {
            return await repository.GetCurrentUserLikeIdsAsync(currentUserId);
        }

        public async Task<IEnumerable<MemberDto>> GetUserLikesAsync(string predicate, int userId)
        {
            return await repository.GetUserLikesAsync(predicate, userId);
        }
    }
}
