using Microsoft.AspNetCore.Identity;

namespace Dating.Core.Models.Identity
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
