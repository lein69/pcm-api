using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.Api.Models
{
    [Table("123_ActivityLogs")]
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Action { get; set; } = null!;

        public string EntityType { get; set; } = null!;

        public int? EntityId { get; set; }

        public string Description { get; set; } = null!;

        public string? IpAddress { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
