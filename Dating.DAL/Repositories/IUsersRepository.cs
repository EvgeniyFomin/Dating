using Dating.Core.Models;

namespace Dating.DAL.Repositories
{
    public interface IUsersRepository
    {
        void UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByNameAsync(string name);
        //-- his
        Task<bool> SaveAllAsync();

        // ---- mine
        Task<User> CreateAsync(User user);
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> IfExists(string userName);
    }
}
