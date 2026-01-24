using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PCM.Api.Models
{
    [Table("123_Members")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public DateTime JoinDate { get; set; }

        public double RankLevel { get; set; }

        public bool IsActive { get; set; } = true;

        // Identity FK
        public string UserId { get; set; } = "";
        public IdentityUser? User { get; set; }

        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";

        public DateTime? DateOfBirth { get; set; }

        public int TotalMatches { get; set; }
        public int WinMatches { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
