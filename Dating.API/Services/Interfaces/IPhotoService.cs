using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<Photo> AddPhotoAsync(IFormFile file, User user);
        Task<bool> DeletePhotoAsync(int photoId, User user);
        PhotoDto MapToDto(Photo photo);
    }
}
