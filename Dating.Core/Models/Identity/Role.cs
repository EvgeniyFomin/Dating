using Microsoft.AspNetCore.Identity;

namespace Dating.Core.Models.Identity
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
