using System.ComponentModel.DataAnnotations;

namespace Dating.Core.Dtos
{
    public class RegisterUserDto
    {
        [MaxLength(100)]
        public required string UserName { get; set; }

        [MaxLength(25)]
        public required string Password { get; set; }
    }
}