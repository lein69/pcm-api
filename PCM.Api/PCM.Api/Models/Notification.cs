﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCM.Api.Enums;

namespace PCM.Api.Models
{
    [Table("123_Notifications")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }


        public int? MemberId { get; set; }
        public Member? Member { get; set; }


        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;


        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? LinkUrl { get; set; }
    }
}
