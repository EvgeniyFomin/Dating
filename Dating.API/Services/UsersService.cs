using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.DAL.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Dating.API.Services
{
    public class UsersService(IUsersRepository userRepository) : IUsersService
    {
        private readonly IUsersRepository _userRepository = userRepository;

        public User CreateUser(RegisterUserDto userDto)
        {
            using var hmac = new HMACSHA512();

            return new User
            {
                UserName = userDto.UserName,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
                PasswordSalt = hmac.Key
            };
        }

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

        public async Task<bool> CheckIfExists(string userName)
        {
            return await _userRepository.IfExists(userName);
        }
    }
}
