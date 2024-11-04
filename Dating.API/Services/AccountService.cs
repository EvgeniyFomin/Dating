using AutoMapper;
using Dating.API.Services.Interfaces;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dating.API.Services
{
    public class AccountService(UserManager<User> userManager, IMapper mapper) : IAccountService
    {
        public async Task<(IdentityResult, User)> CreateAccountAsync(RegisterUserDto userDto)
        {
            var user = mapper.Map<User>(userDto);

            return (await userManager.CreateAsync(user, userDto.Password), user);
        }

        public async Task<bool> CheckIfExistsAsync(string userName)
        {
            return await userManager.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper());
        }

        public async Task<bool> CheckIfPasswordValid(User user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<User?> GetByNameAsync(string userName)
        {
            return await userManager.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpper());
        }

        public async Task<IdentityResult> UpdateLastActivityDateAsync(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                user.LastActive = DateTime.UtcNow;
                return await userManager.UpdateAsync(user);
            }

            return IdentityResult.Failed([new IdentityError { Description = "User was not updated" }]);
        }
    }
}
