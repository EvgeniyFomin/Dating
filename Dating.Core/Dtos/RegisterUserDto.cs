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
    }
}