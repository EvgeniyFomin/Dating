using Dating.Core.Models;

namespace Dating.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> GetByIdAsynv(int id);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(User user);
    }
}
