using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;

namespace Dating.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<PagedList<MemberDto>> GetPagedMemberDtosAsync(UserFilteringParameters parameters);
        Task<MemberDto?> GetMemberDtoByIdAsync(int id, bool isCurrentUser);
        Task<User?> GetByIdAsync(int id);
        Task<bool> UpdateUserAsync(MemberUpdateDto memberDto, string userName);
        Task<bool> AddPhotoToUserAsync(User user, Photo photo);
        Task<bool> SetPhotoAsMainToUserAsync(User user, int photoId);
        Task<(bool, string?)> DeletePhotoReturnPublicIdAsync(User user, int photoId);
    }
}