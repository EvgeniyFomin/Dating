using System.ComponentModel.DataAnnotations;

namespace Dating.Core.Dtos
{
    public class RegisterUserDto : LoginUserDto
    {
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