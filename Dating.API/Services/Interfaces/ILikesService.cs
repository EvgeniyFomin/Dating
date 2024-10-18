using Dating.Core.Dtos;
using Dating.Core.Models.Pagination;

namespace Dating.API.Services.Interfaces
{
    public interface ILikesService
    {
        Task<bool> LikeToggle(int sourceUserId, int targetUserId);
        Task<IEnumerable<int>> GetUserLikeIdsAsync(int currentUserId);
        Task<PagedList<MemberDto>> GetUserLikesAsync(LikesFilteringParameters parameters);
    }
}
