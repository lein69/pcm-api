﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.Api.Models
{
    [Table("123_News")]
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public bool IsPinned { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
