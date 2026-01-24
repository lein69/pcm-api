namespace PCM.Api.DTOs
{
    public class ActivityLogDto
    {
        public string Action { get; set; } = "";
        public string EntityType { get; set; } = "";
        public int? EntityId { get; set; }
        public string Description { get; set; } = "";
        public string? IpAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}