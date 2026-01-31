﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCM.Api.Enums;

namespace PCM.Api.Models
{
    [Table("123_Challenges")]
    public class Challenge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public ChallengeType Type { get; set; }

        public GameMode GameMode { get; set; }

        public ChallengeStatus Status { get; set; }


        public int? Config_TargetWins { get; set; }

        public int CurrentScore_TeamA { get; set; }

        public int CurrentScore_TeamB { get; set; }


        public decimal EntryFee { get; set; }

        public decimal PrizePool { get; set; }


        public int CreatedById { get; set; }
        public Member CreatedBy { get; set; } = null!;


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }
}
