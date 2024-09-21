using AutoMapper;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.DAL.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace Dating.API.Services
{
    public class UsersService(IUsersRepository userRepository, IMapper mapper) : IUsersService
    {
        private readonly IUsersRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public User CreateUser(RegisterUserDto userDto)
        {
            using var hmac = new HMACSHA512();

            return new User
            {
                UserName = userDto.UserName,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
                PasswordSalt = hmac.Key,
                // ===== to disable error
                KnownAs = "",
                City = "",
                Country = ""
            };
        }

        public async Task<User> AddAsync(User user)
        {
            return await _userRepository.CreateAsync(user);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await _userRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<MemberDto>> GetAllDtosAsync()
        {
            var result = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<MemberDto>>(result);
        }

        public async Task<MemberDto> GetDtoByIdAsync(int id)
        {
            var result = await _userRepository.GetByIdAsync(id);

            return _mapper.Map<MemberDto>(result);
        }

        public async Task<User?> GetByNameAsync(string userName)
        {
            return await _userRepository.GetByNameAsync(userName);
        }
        public async Task<MemberDto> GetDtoByNameAsync(string userName)
        {
            var result = await _userRepository.GetByNameAsync(userName);

            return _mapper.Map<MemberDto>(result);
        }

        public bool CheckIfPasswordValid(User user, string password)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.Password[i])
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CheckIfExistsAsync(string userName)
        {
            return await _userRepository.IfExists(userName);
        }
    }
}