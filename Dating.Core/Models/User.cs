using Dating.Core.Enums;
using Dating.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Dating.Core.Models
{
    public class User : IdentityUser<int>
    {
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public required string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = [];

        // navigation properties
        public List<Photo> Photos { get; set; } = [];
        public List<UserLike> LikedByUsers { get; set; } = [];
        public List<UserLike> LikedUsers { get; set; } = [];
        public List<Message> MessagesSent { get; set; } = [];
        public List<Message> MessagesReceived { get; set; } = [];
    }
}
