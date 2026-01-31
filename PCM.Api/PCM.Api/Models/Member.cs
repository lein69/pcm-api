﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PCM.Api.Models
{
    [Table("123_Members")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }

        public double RankLevel { get; set; }

        public bool IsActive { get; set; } = true;

        // Identity FK
        public string UserId { get; set; } = string.Empty;
        public IdentityUser User { get; set; } = null!;

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public int TotalMatches { get; set; }
        public int WinMatches { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
