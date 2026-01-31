using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Models;
using PCM.Api.Enums;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Bookings
                .Include(b => b.Court)
                .Include(b => b.Member)
                .ToListAsync());
        }

        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots(int courtId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var existingBookings = await _context.Bookings
                .Where(b => b.CourtId == courtId && b.StartTime >= startOfDay && b.StartTime < endOfDay && b.Status != BookingStatus.Cancelled)
                .ToListAsync();

            var availableSlots = new List<object>();
            var currentTime = startOfDay.AddHours(6); // Start from 6 AM
            var endTime = startOfDay.AddHours(22); // End at 10 PM

            while (currentTime < endTime)
            {
                var slotEnd = currentTime.AddHours(1);
                var isAvailable = !existingBookings.Any(b =>
                    (b.StartTime < slotEnd && b.EndTime > currentTime));

                availableSlots.Add(new
                {
                    StartTime = currentTime,
                    EndTime = slotEnd,
                    IsAvailable = isAvailable
                });

                currentTime = slotEnd;
            }

            return Ok(availableSlots);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            // Validation: Check for overlapping bookings
            var overlappingBooking = await _context.Bookings
                .AnyAsync(b => b.CourtId == booking.CourtId &&
                              b.Status != BookingStatus.Cancelled &&
                              b.StartTime < booking.EndTime &&
                              b.EndTime > booking.StartTime);

            if (overlappingBooking)
            {
                return BadRequest("Court is already booked for this time slot.");
            }

            if (booking.StartTime >= booking.EndTime)
            {
                return BadRequest("End time must be after start time.");
            }

            if (booking.StartTime < DateTime.UtcNow)
            {
                return BadRequest("Cannot book in the past.");
            }

            booking.CreatedDate = DateTime.UtcNow;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Booking booking)
        {
            if (id != booking.Id)
                return BadRequest();

            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
