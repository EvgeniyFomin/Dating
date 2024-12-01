using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<Photo> AddPhotoAsync(IFormFile file, User user);
        Task<bool> DeleteUsersPhotoAsync(int photoId, User user);
        Task<bool> RemovePhotoAsync(int photoId);
        Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotoDtosAsync();
        Task<bool> ApprovePhotoAsync(int photoId);
        PhotoDto MapToDto(Photo photo);
    }
}
