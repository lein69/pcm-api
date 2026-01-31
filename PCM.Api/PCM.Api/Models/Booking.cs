﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PCM.Api.Enums;

namespace PCM.Api.Models
{
    [Table("123_Bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        // FK Court
        public int CourtId { get; set; }
        public Court Court { get; set; } = null!;

        // FK Member
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
