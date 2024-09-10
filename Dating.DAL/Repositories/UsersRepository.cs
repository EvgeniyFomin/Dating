using Dating.Core.Models;
using Dating.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Repositories
{
    public class UsersRepository(DataContext dataContext) : IUsersRepository
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<User> CreateAsync(User user)
        {
            var result = await _dataContext.Users.AddAsync(user);

            await _dataContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var user = await GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            _ = _dataContext.Remove(user);
            _ = await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public Task<User> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IfExists(string userName)
        {
            return await _dataContext.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}
