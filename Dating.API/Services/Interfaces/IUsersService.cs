using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(int id);

        Task<User> AddAsync(User user);

        Task<bool> DeleteByIdAsync(int id);

        User CreateUser(RegisterUserDto userDto);

        Task<bool> CheckIfExists(string userName);

        Task<User> GetByName(string userName);

        bool CheckIfPasswordValid(User user, string password);
    }
}
