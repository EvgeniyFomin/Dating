﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Core.Models
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public bool IsApproved { get; set; }

        // navigation property
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
