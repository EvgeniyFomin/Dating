﻿using Dating.Core.Enums;
using Dating.Core.Extensions;

namespace Dating.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public byte[] Password { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];

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
        public List<Photo> Photos { get; set; } = [];

        public int GetAge()
        {
            return DateOfBirth.GetAge();
        }
    }
}
