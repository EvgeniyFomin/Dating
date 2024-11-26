using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.DAL.Context;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Repositories
{
    public class PhotosRepository(DataContext context, IMapper mapper) : IPhotosRepository
    {
        public async Task<Photo?> GetById(int photoId)
        {
            return await context.Photos.FindAsync(photoId);
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
        {
            return await context.Photos
                .Where(x => x.IsApproved == false)
                .ProjectTo<PhotoForApprovalDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void Remove(Photo photo)
        {
            context.Photos.Remove(photo);
        }
    }
}
