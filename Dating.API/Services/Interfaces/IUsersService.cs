using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<MemberDto>> GetAllMemberDtosAsync();
        Task<MemberDto?> GetMemberDtoByIdAsync(int id);
        Task<MemberDto?> GetMemberDtoByNameAsync(string userName);
        Task<User?> GetByNameAsync(string userName);
        Task<User> AddAsync(User user);
        Task<bool> CheckIfExistsAsync(string userName);
        Task<User> CreateUser(RegisterUserDto userDto);
        bool CheckIfPasswordValid(User user, string password);
        Task<bool> UpdateUser(MemberUpdateDto memberDto, string userName);
    }
}
