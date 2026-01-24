using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.Api.Models
{
    [Table("123_Courts")]
    public class Court
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Booking>? Bookings { get; set; }
    }
}
