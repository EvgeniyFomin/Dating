using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Dating.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Dating.DAL.Repositories
{
    public class UsersRepository(DataContext dataContext, IMapper mapper) : IUsersRepository
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IMapper _mapper = mapper;

        public void UpdateAsync(User user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dataContext.Users
                .Include(x => x.Photos)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dataContext.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _dataContext.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName.ToLower() == name.ToLower());
        }

        // members
        public async Task<IEnumerable<MemberDto>> GetAllMemberDtosAsync()
        {
            return await _dataContext.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MemberDto?> GetMemberDtoByName(string name)
        {
            return await _dataContext.Users
                 .Where(x => x.UserName.ToLower() == name.ToLower())
                 .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                 .SingleOrDefaultAsync();
        }

        public async Task<MemberDto?> GetMemberDtoById(int id)
        {
            return await _dataContext.Users
                   .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                   .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        // ---- mine
        public async Task<User> CreateAsync(User user)
        {
            var result = await _dataContext.Users.AddAsync(user);

            await _dataContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var user = await GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            _ = _dataContext.Remove(user);
            _ = await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IfExists(string userName)
        {
            return await _dataContext.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}
