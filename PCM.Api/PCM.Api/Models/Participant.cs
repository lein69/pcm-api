using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCM.Api.Enums;

namespace PCM.Api.Models
{
    [Table("123_Participants")]
    public class Participant
    {
        [Key]
        public int Id { get; set; }


        public int ChallengeId { get; set; }
        public Challenge? Challenge { get; set; }


        public int MemberId { get; set; }
        public Member? Member { get; set; }


        public TeamSide Team { get; set; }


        public bool EntryFeePaid { get; set; }

        public decimal EntryFeeAmount { get; set; }


        public DateTime JoinedDate { get; set; }


        public ParticipantStatus Status { get; set; }
    }
}
