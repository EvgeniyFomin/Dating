using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Microsoft.Extensions.Options;

namespace Dating.API.Services.CloudinaryService
{
    public class CloudinaryService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        public CloudinaryService(IOptions<CloudinarySettings> config, IMapper mapper)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);

            _mapper = mapper;
        }

        public async Task<Photo?> AddPhotoAsync(IFormFile file)
        {
            if (file.Length <= 0) throw new Exception("Photo file is empty");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "dating"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return GetPhotoFromResult(uploadResult);
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deletionParams);
        }

        public PhotoDto MapToDto(Photo photo)
        {
            return _mapper.Map<PhotoDto>(photo);
        }

        private static Photo GetPhotoFromResult(ImageUploadResult result)
        {
            return new Photo { Url = result.SecureUrl.AbsoluteUri, PublicId = result.PublicId };
        }
    }
}
