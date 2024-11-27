using AutoMapper;
using Dating.API.Services.CloudinaryService;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.DAL.Repositories.Interfaces;

namespace Dating.API.Services
{
    public class PhotoService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IMapper mapper) : IPhotoService
    {
        public async Task<Photo> AddPhotoAsync(IFormFile file, User user)
        {
            var photo = await cloudinaryService.UploadPhotoAsync(file)
                ?? throw new Exception("Photo was not uploaded");

            if (user.Photos.Count == 0)
                photo.IsMain = true;

            user.Photos.Add(photo);

            return await unitOfWork.Complete()
                ? photo
                : throw new Exception("User was not updated - photo not added");
        }

        public async Task<bool> DeletePhotoAsync(int photoId, User user)
        {
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) throw new Exception("photo cannot be deleted from user");

            user.Photos.Remove(photo);

            if (await unitOfWork.Complete())
            {
                var deletionResult = await cloudinaryService.DeletePhotoAsync(photo.PublicId!);
                return deletionResult.Error == null;
            };

            return false;
        }

        public PhotoDto MapToDto(Photo photo)
        {
            return mapper.Map<PhotoDto>(photo);
        }
    }
}
