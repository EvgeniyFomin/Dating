using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        // Get user
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByNameAsync(string name);

        // Get member Dto 
        Task<PagedList<MemberDto>> GetMemberDtosAsync(PaginationParameters parameters);
        Task<MemberDto?> GetMemberDtoById(int id);
        Task<MemberDto?> GetMemberDtoByName(string name);

        Task<User> CreateAsync(User user);
        Task<bool> IfExists(string userName);
        Task<bool> UpdateLastActiveDateAsync(int id);
        Task<bool> SaveAllAsync();
    }
}
