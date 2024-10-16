using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetLikeAsync(int sourceUserId, int targetUserId);
        Task<IEnumerable<MemberDto>> GetUserLikesAsync(string predicate, int userId);
        Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int currentUserId);
        Task AddLikeAsync(UserLike userLike);
        Task RemoveLikeAsync(UserLike userLike);
        Task<bool> SaveChangesAsync();
    }
}
