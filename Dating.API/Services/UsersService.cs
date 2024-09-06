using Dating.Core.Models;
using Dating.DAL.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dating.API.Services
{
    public class UsersService(IUsersRepository userRepository) : IUsersService
    {
        private readonly IUsersRepository _userRepository = userRepository;

        public async Task<User> AddAsync(User user)
        {
            return await _userRepository.CreateAsync(user);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var result = await _userRepository.DeleteByIdAsync(id);

            return result;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();

        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }
}
