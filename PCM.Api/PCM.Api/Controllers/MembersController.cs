using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Models;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Members.ToListAsync());
        }

        [HttpGet("top-ranking")]
        public async Task<IActionResult> GetTopRanking(int limit = 5)
        {
            var topMembers = await _context.Members
                .Where(m => m.IsActive)
                .OrderByDescending(m => m.RankLevel)
                .Take(limit)
                .ToListAsync();

            return Ok(topMembers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
                return NotFound();

            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return Ok(member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Member member)
        {
            if (id != member.Id)
                return BadRequest();

            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
                return NotFound();

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
