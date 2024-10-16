using AutoMapper;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
using Dating.DAL.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Dating.API.Services
{
    public class UsersService(IUsersRepository userRepository, IMapper mapper) : IUsersService
    {
        private readonly IUsersRepository _userRepository = userRepository;

        public async Task<User> CreateUserAsync(RegisterUserDto userDto)
        {
            var user = mapper.Map<User>(userDto);

            using var hmac = new HMACSHA512();
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password!));
            user.PasswordSalt = hmac.Key;

            return await AddAsync(user);
        }

        public async Task<User> AddAsync(User user)
        {
            return await _userRepository.CreateAsync(user);
        }

        public async Task<PagedList<MemberDto>> GetPagedMemberDtosAsync(PaginationParameters parameters)
        {
            return await _userRepository.GetMemberDtosAsync(parameters);
        }

        public async Task<MemberDto?> GetMemberDtoByIdAsync(int id)
        {
            return await _userRepository.GetMemberDtoById(id);
        }

        public async Task<User?> GetByNameAsync(string userName)
        {
            return await _userRepository.GetByNameAsync(userName);
        }

        public async Task<MemberDto?> GetMemberDtoByNameAsync(string userName)
        {
            return await _userRepository.GetMemberDtoByName(userName);
        }

        public async Task<bool> CheckIfExistsAsync(string userName)
        {
            return await _userRepository.IfExists(userName);
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

        public async Task<bool> UpdateUserAsync(MemberUpdateDto memberDto, string userName)
        {
            var user = await _userRepository.GetByNameAsync(userName);

            mapper.Map(memberDto, user);

            return await _userRepository.SaveAllAsync();
        }

        public async Task<bool> AddPhotoToUserAsync(User user, Photo photo)
        {
            if (user.Photos.Count == 0)
                photo.IsMain = true;

            user.Photos.Add(photo);

            return await _userRepository.SaveAllAsync();
        }

        public async Task<bool> SetPhotoAsMainToUserAsync(User user, int photoId)
        {
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return await Task.FromResult(false);

            user.Photos.Where(x => x.IsMain)?.ToList()?.ForEach(x => x.IsMain = false);

            photo.IsMain = true;

            return await _userRepository.SaveAllAsync();
        }

        public async Task<(bool, string?)> DeletePhotoReturnPublicIdAsync(User user, int photoId)
        {
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return (false, null);

            user.Photos.Remove(photo);

            return await _userRepository.SaveAllAsync()
                ? (true, photo.PublicId)
                : (false, null);
        }

        public async Task<bool> UpdateLastActivityDateAsync(int userId)
        {
            return await _userRepository.UpdateLastActiveDateAsync(userId);
        }
    }
}