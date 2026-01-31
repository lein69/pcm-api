﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.Api.Models
{
    [Table("123_MatchScores")]
    public class MatchScore
    {
        [Key]
        public int Id { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

        public int SetNumber { get; set; }

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        public bool IsFinalSet { get; set; }
    }
}
