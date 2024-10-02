using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<MemberDto>> GetAllMemberDtosAsync();
        Task<MemberDto?> GetMemberDtoByIdAsync(int id);
        Task<MemberDto?> GetMemberDtoByNameAsync(string userName);
        Task<User?> GetByNameAsync(string userName);
        Task<User> AddAsync(User user);
        Task<bool> CheckIfExistsAsync(string userName);
        Task<User> CreateUserAsync(RegisterUserDto userDto);
        bool CheckIfPasswordValid(User user, string password);
        Task<bool> UpdateUserAsync(MemberUpdateDto memberDto, string userName);
        Task<bool> AddPhotoToUserAsync(User user, Photo photo);
        Task<bool> SetPhotoAsMainToUserAsync(User user, int photoId);
        Task<string?> DeletePhotoReturnPublicIdAsync(User user, int photoId);
    }
}
