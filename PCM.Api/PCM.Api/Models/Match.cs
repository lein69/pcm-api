using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCM.Api.Enums;

namespace PCM.Api.Models
{
    [Table("123_Matches")]
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsRanked { get; set; }


        // ================= CHALLENGE =================
        public int? ChallengeId { get; set; }

        public Challenge? Challenge { get; set; }


        // ================= FORMAT =================
        public MatchFormat MatchFormat { get; set; }


        // ================= TEAM 1 =================
        public int Team1_Player1Id { get; set; }

        public Member? Team1_Player1 { get; set; }


        public int? Team1_Player2Id { get; set; }

        public Member? Team1_Player2 { get; set; }


        // ================= TEAM 2 =================
        public int Team2_Player1Id { get; set; }

        public Member? Team2_Player1 { get; set; }


        public int? Team2_Player2Id { get; set; }

        public Member? Team2_Player2 { get; set; }


        // ================= RESULT =================
        public WinningSide WinningSide { get; set; }
    }
}
