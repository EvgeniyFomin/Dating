using AutoMapper;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dating.API.Services
{
    // TODO - separate account's stuff and user's stuff
    public class UsersService(UserManager<User> userManager, IMapper mapper, IUsersRepository userRepository) : IUsersService
    {
        public async Task<(IdentityResult, User)> CreateUserAsync(RegisterUserDto userDto)
        {
            var user = mapper.Map<User>(userDto);

            return (await userManager.CreateAsync(user, userDto.Password), user);
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
            return await userManager.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
        }

        public async Task<MemberDto?> GetMemberDtoByNameAsync(string userName)
        {
            return await userRepository.GetMemberDtoByName(userName);
        }

        public async Task<bool> CheckIfExistsAsync(string userName)
        {
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper());
        }

        public async Task<bool> CheckIfPasswordValid(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
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