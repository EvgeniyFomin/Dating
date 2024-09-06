using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(int id);

        Task<User> AddAsync(User user);

        Task<bool> DeleteByIdAsync(int id);
    }
}
