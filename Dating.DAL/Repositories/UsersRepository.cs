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
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await dataContext.Users
                .Include(x => x.Photos)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await dataContext.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await dataContext.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.NormalizedUserName == name.ToUpper());
        }

        public async Task<bool> SaveAllAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public async Task<User> CreateAsync(User user)
        {
            var result = await dataContext.Users.AddAsync(user);

            await dataContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> IfExists(string userName)
        {
            return await dataContext.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper());
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

        public async Task<MemberDto?> GetMemberDtoByName(string name)
        {
            return await dataContext.Users
                 .Where(x => x.NormalizedUserName == name.ToUpper())
                 .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                 .SingleOrDefaultAsync();
        }

        public async Task<MemberDto?> GetMemberDtoById(int id)
        {
            return await dataContext.Users
                   .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                   .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateLastActiveDateAsync(int id)
        {
            var user = dataContext.Users.First(a => a.Id == id);

            user.LastActive = DateTime.UtcNow;

            return await SaveAllAsync();
        }
    }
}
