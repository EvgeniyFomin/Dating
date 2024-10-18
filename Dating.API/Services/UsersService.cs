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
            return await userRepository.CreateAsync(user);
        }

        public async Task<PagedList<MemberDto>> GetPagedMemberDtosAsync(UserFilteringParameters parameters)
        {
            return await userRepository.GetMemberDtosAsync(parameters);
        }

        public async Task<MemberDto?> GetMemberDtoByIdAsync(int id)
        {
            return await userRepository.GetMemberDtoById(id);
        }

        public async Task<User?> GetByNameAsync(string userName)
        {
            return await userRepository.GetByNameAsync(userName);
        }

        public async Task<MemberDto?> GetMemberDtoByNameAsync(string userName)
        {
            return await userRepository.GetMemberDtoByName(userName);
        }

        public async Task<bool> CheckIfExistsAsync(string userName)
        {
            return await userRepository.IfExists(userName);
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
            var user = await userRepository.GetByNameAsync(userName);

            mapper.Map(memberDto, user);

            return await userRepository.SaveAllAsync();
        }

        public async Task<bool> AddPhotoToUserAsync(User user, Photo photo)
        {
            if (user.Photos.Count == 0)
                photo.IsMain = true;

            user.Photos.Add(photo);

            return await userRepository.SaveAllAsync();
        }

        public async Task<bool> SetPhotoAsMainToUserAsync(User user, int photoId)
        {
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return await Task.FromResult(false);

            user.Photos.Where(x => x.IsMain)?.ToList()?.ForEach(x => x.IsMain = false);

            photo.IsMain = true;

            return await userRepository.SaveAllAsync();
        }

        public async Task<(bool, string?)> DeletePhotoReturnPublicIdAsync(User user, int photoId)
        {
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return (false, null);

            user.Photos.Remove(photo);

            return await userRepository.SaveAllAsync()
                ? (true, photo.PublicId)
                : (false, null);
        }

        public async Task<bool> UpdateLastActivityDateAsync(int userId)
        {
            return await userRepository.UpdateLastActiveDateAsync(userId);
        }
    }
}