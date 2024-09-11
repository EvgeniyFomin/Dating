using Dating.Core.Models;

namespace Dating.API.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
