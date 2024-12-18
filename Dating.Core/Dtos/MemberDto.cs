﻿using Dating.Core.Enums;

namespace Dating.Core.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string? MainPhotoUrl { get; set; }
        public string? KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public List<PhotoDto>? Photos { get; set; }
    }
}
