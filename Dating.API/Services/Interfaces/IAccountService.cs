using AutoMapper;
using Dating.Core.Dtos;
using Dating.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Dating.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(IdentityResult, User)> CreateAccountAsync(RegisterUserDto userDto);
        Task<bool> CheckIfExistsAsync(string userName);
        Task<bool> CheckIfPasswordValid(User user, string password);
        Task<User?> GetByNameAsync(string userName);
        Task<IdentityResult> UpdateLastActivityDateAsync(int id);
    }
}
