using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetByIdAsync(int id, bool isUserCurrent);
        Task<User?> GetByNameAsync(string name);
        Task<User?> GetByPhotoIdAsync(int photoId);

        // member
        Task<PagedList<MemberDto>> GetMemberDtosAsync(UserFilteringParameters parameters);
        Task<MemberDto?> GetMemberDtoByIdAsync(int id, bool isCurrentUser);
    }
}
