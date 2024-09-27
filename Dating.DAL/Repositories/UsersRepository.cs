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

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<User> CreateAsync(User user)
        {
            var result = await _dataContext.Users.AddAsync(user);

            await _dataContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> IfExists(string userName)
        {
            return await _dataContext.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
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
    }
}
