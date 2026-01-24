using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.DTOs;
using PCM.Api.Models;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.ActivityLogs.ToListAsync());
        }

        // ================= CREATE ACTIVITY LOG =================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityLogDto dto)
        {
            var activityLog = new ActivityLog
            {
                Action = dto.Action,
                EntityType = dto.EntityType,
                EntityId = dto.EntityId,
                Description = dto.Description,
                IpAddress = dto.IpAddress,
                CreatedDate = dto.CreatedDate,
                UserId = "system" // or get from context
            };
            _context.ActivityLogs.Add(activityLog);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = activityLog.Id }, activityLog);
        }

        // ================= UPDATE ACTIVITY LOG =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityLogDto dto)
        {
            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog == null)
            {
                return NotFound();
            }
            activityLog.Action = dto.Action;
            activityLog.EntityType = dto.EntityType;
            activityLog.EntityId = dto.EntityId;
            activityLog.Description = dto.Description;
            activityLog.IpAddress = dto.IpAddress;
            activityLog.CreatedDate = dto.CreatedDate;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ================= DELETE ACTIVITY LOG =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog == null)
            {
                return NotFound();
            }
            _context.ActivityLogs.Remove(activityLog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
