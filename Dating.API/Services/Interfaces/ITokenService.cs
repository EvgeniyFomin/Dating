using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
