using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<MemberDto>> GetAllDtosAsync();
        Task<MemberDto> GetDtoByIdAsync(int id);
        Task<User?> GetByNameAsync(string userName);
        Task<MemberDto> GetDtoByNameAsync(string userName);
        Task<User> AddAsync(User user);
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> CheckIfExistsAsync(string userName);
        User CreateUser(RegisterUserDto userDto);
        bool CheckIfPasswordValid(User user, string password);
    }
}
