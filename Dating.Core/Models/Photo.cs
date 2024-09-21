﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Dating.Core.Models
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }

        // navigation property
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
