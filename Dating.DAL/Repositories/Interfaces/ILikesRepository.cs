using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetLikeAsync(int sourceUserId, int targetUserId);
        Task<PagedList<MemberDto>> GetUserLikesAsync(LikesFilteringParameters parameters);
        Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int userId);
        Task AddLikeAsync(UserLike userLike);
        void RemoveLike(UserLike userLike);
        Task<bool> SaveChangesAsync();
    }
}
