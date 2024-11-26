using Dating.Core.Dtos;
using Dating.Core.Models;

namespace Dating.DAL.Repositories.Interfaces
{
    public interface IPhotosRepository
    {
        Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
        Task<Photo?> GetById(int photoId);
        void Remove(Photo photo);
    }
}
