using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.DAL.Repositories
{
    public interface IUsersRepository
    {
        // Get user
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByNameAsync(string name);

        // Get member Dto 
        Task<IEnumerable<MemberDto>> GetAllMemberDtosAsync();
        Task<MemberDto?> GetMemberDtoById(int id);
        Task<MemberDto?> GetMemberDtoByName(string name);

        Task<User> CreateAsync(User user);
        Task<bool> IfExists(string userName);
        Task<bool> SaveAllAsync();
    }
}
