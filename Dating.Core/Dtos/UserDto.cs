using Dating.Core.Enums;

namespace Dating.Core.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string KnownAs { get; set; }
        public required string Token { get; set; }
        public string? PhotoUrl { get; set; }
        public required Gender Gender { get; set; }
    }
}
