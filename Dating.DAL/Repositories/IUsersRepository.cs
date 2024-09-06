using Dating.Core.Models;

namespace Dating.DAL.Repositories
{
    public interface IUsersRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteByIdAsync(int id);
    }
}
