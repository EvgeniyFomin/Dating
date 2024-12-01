using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface IPhotosRepository
    {
        Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotosAsync();
        Task<Photo?> GetByIdAsync(int photoId);
        Task<bool> ApprovePhotoAsync(int photoId);
        void Remove(Photo photo);
    }
}
