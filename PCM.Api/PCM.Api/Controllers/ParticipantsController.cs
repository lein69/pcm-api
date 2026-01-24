using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Models;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParticipantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Participants
                .Include(p => p.Member)
                .Include(p => p.Challenge)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.Participants.FindAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Participant p)
        {
            p.JoinedDate = DateTime.Now;

            _context.Participants.Add(p);
            await _context.SaveChangesAsync();

            return Ok(p);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Participant p)
        {
            if (id != p.Id)
                return BadRequest();

            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(p);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Participants.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Participants.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
