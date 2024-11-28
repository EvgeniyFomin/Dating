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
        public async Task<Photo?> GetByIdAsync(int photoId)
        {
            return await context.Photos
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(x => x.Id == photoId);
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotosAsync()
        {
            return await context.Photos
                .IgnoreQueryFilters()
                .Where(x => x.IsApproved == false)
                .ProjectTo<PhotoForApprovalDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> ApprovePhotoAsync(int photoId)
        {
            Resu

            return await context.Photos
                .IgnoreQueryFilters()
                .Where(x => x.Id == photoId && x.IsApproved == false)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsApproved, true))
                > 0;
        }

        public void Remove(Photo photo)
        {
            context.Photos.Remove(photo);
        }
    }
}
