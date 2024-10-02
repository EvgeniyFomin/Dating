using System.Security.Claims;

namespace Dating.API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("Cannot get the username from token");
        }
    }
}
