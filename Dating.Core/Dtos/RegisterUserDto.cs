using Dating.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Dating.Core.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(8, MinimumLength = 3)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string? Gender { get; set; }

        [Required]
        public string? KnownAs { get; set; }

        [Required]
        public string? DateOfBirth { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }
    }
}