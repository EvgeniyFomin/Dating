using Dating.API.Services.Interfaces;
using Dating.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dating.API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        private const string KEY = "TokenKey";
        private const string ERROR_MESSAGE = "Cannot get tokenKey from appsettings";
        private const string TOKENKEY_IS_SHORT = "Your tokenKey should be longer than 64 chars";
        private const string NO_USERNAME_FOR_USER = "No username for user";

        public string CreateToken(User user)
        {
            var tokenKey = config[KEY] ?? throw new Exception(ERROR_MESSAGE);

            if (tokenKey.Length < 64) throw new Exception(TOKENKEY_IS_SHORT);
            if (user.UserName == null) throw new Exception(NO_USERNAME_FOR_USER);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
            };


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
