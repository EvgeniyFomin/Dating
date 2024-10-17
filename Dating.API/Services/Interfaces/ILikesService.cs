using Dating.Core.Dtos;

namespace Dating.API.Services.Interfaces
{
    public interface ILikesService
    {
        Task<bool> LikeToggle(int sourceUserId, int targetUserId);
        Task<IEnumerable<int>> GetUserLikeIdsAsync(int currentUserId);
        Task<IEnumerable<MemberDto>> GetUserLikesAsync(string predicate, int userId);
    }
}
