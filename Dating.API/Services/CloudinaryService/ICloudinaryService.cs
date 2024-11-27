using CloudinaryDotNet.Actions;
using Dating.Core.Models;

namespace Dating.API.Services.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<Photo?> UploadPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
