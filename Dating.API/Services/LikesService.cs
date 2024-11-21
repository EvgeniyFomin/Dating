using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
using Dating.DAL.Repositories.Interfaces;

namespace Dating.API.Services
{
    public class LikesService(IUnitOfWork unitOfWork) : ILikesService
    {
        private readonly ILikesRepository _likesRepository = unitOfWork.LikesRepository;

        public async Task<bool> LikeToggle(int sourceUserId, int targetUserId)
        {
            var existingLike = await _likesRepository.GetLikeAsync(sourceUserId, targetUserId);

            if (existingLike == null)
            {
                var like = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };

                await _likesRepository.AddLikeAsync(like);
            }
            else
            {
                _likesRepository.RemoveLike(existingLike);
            }

            return await unitOfWork.Complete();
        }

        public async Task<IEnumerable<int>> GetUserLikeIdsAsync(int currentUserId)
        {
            return await _likesRepository.GetCurrentUserLikeIdsAsync(currentUserId);
        }

        public async Task<PagedList<MemberDto>> GetUserLikesAsync(LikesFilteringParameters parameters)
        {
            return await _likesRepository.GetUserLikesAsync(parameters);
        }
    }
}
