using CloudinaryDotNet.Actions;
using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface IPhotoService
    {
        Task<Photo?> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        PhotoDto MapToDto(Photo photo);
    }
}
