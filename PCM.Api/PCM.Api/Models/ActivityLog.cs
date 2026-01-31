using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.Api.Models
{
    [Table("123_ActivityLogs")]
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string EntityType { get; set; } = string.Empty;

        public int? EntityId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? IpAddress { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
