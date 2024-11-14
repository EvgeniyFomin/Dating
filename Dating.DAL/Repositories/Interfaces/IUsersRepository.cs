using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByNameAsync(string name);

        // member
        Task<PagedList<MemberDto>> GetMemberDtosAsync(UserFilteringParameters parameters);
        Task<MemberDto?> GetMemberDtoById(int id);
        Task<MemberDto?> GetMemberDtoByName(string name);
    }
}
