using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.Core.Models.Pagination;
using Dating.DAL.Context;
using Dating.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Repositories
{
    public class UsersRepository(DataContext dataContext, IMapper mapper) : IUsersRepository
    {
        public async Task<User?> GetByIdAsync(int id, bool isUserCurrent)
        {
            var query = dataContext.Users
                .Where(x => x.Id == id)
                .Include(x => x.Photos)
                .AsQueryable();

            if (isUserCurrent) query = query.IgnoreQueryFilters();

            return await query.SingleOrDefaultAsync();
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await dataContext.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.NormalizedUserName == name.ToUpper());
        }

        public async Task<User?> GetByPhotoIdAsync(int photoId)
        {
            return await dataContext.Users
                  .Include(p => p.Photos)
                  .IgnoreQueryFilters()
                  .Where(x => x.Photos.Any(p => p.Id == photoId))
                  .FirstOrDefaultAsync();
        }

        // members
        public async Task<PagedList<MemberDto>> GetMemberDtosAsync(UserFilteringParameters parameters)
        {
            var query = dataContext.Users.AsQueryable();
            query = query.Where(x => x.UserName != parameters.CurrentUserName);

            if (parameters.Gender != null)
            {
                query = query.Where(x => x.Gender == parameters.Gender);
            }

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-parameters.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-parameters.MinAge));

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            query = parameters.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), parameters);
        }

        public async Task<MemberDto?> GetMemberDtoByIdAsync(int id, bool isCurrentUser)
        {
            var query = dataContext.Users
                            .Where(x => x.Id == id)
                            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                            .AsQueryable();

            if (isCurrentUser) query = query.IgnoreQueryFilters();

            return await query.SingleOrDefaultAsync();
        }


    }
}