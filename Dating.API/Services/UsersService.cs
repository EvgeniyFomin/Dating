using AutoMapper;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
using Dating.DAL.Repositories.Interfaces;

namespace Dating.API.Services
{
    public class UsersService(IUnitOfWork unitOfWork, IMapper mapper) : IUsersService
    {
        private readonly IUsersRepository _usersRepository = unitOfWork.UsersRepository;

        public async Task<PagedList<MemberDto>> GetPagedMemberDtosAsync(UserFilteringParameters parameters)
        {
            return await _usersRepository.GetMemberDtosAsync(parameters);
        }

        public async Task<MemberDto?> GetMemberDtoByIdAsync(int id)
        {
            return await _usersRepository.GetMemberDtoById(id);
        }

        public async Task<User?> GetByNameAsync(string userName)
        {
            return await _usersRepository.GetByNameAsync(userName);
        }

        public async Task<MemberDto?> GetMemberDtoByNameAsync(string userName)
        {
            return await _usersRepository.GetMemberDtoByName(userName);
        }

        public async Task<bool> UpdateUserAsync(MemberUpdateDto memberDto, string userName)
        {
            var user = await _usersRepository.GetByNameAsync(userName);

            mapper.Map(memberDto, user);

            return await unitOfWork.Complete();
        }

        public async Task<bool> AddPhotoToUserAsync(User user, Photo photo)
        {
            if (user.Photos.Count == 0)
                photo.IsMain = true;

            user.Photos.Add(photo);

            return await unitOfWork.Complete();
        }

        public async Task<bool> SetPhotoAsMainToUserAsync(User user, int photoId)
        {
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return await Task.FromResult(false);

            user.Photos.Where(x => x.IsMain)?.ToList()?.ForEach(x => x.IsMain = false);

            photo.IsMain = true;

            return await unitOfWork.Complete();
        }

        public async Task<(bool, string?)> DeletePhotoReturnPublicIdAsync(User user, int photoId)
        {
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return (false, null);

            user.Photos.Remove(photo);

            return await unitOfWork.Complete()
                ? (true, photo.PublicId)
                : (false, null);
        }
    }
}