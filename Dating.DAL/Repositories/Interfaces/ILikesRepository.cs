using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetLikeAsync(int sourceUserId, int targetUserId);
        Task<IEnumerable<MemberDto>> GetUserLikesAsync(string predicate, int userId);
        Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int userId);
        Task AddLikeAsync(UserLike userLike);
        void RemoveLike(UserLike userLike);
        Task<bool> SaveChangesAsync();
    }
}
