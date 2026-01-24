using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Models;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourtsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Courts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Courts.ToListAsync());
        }

        // GET: api/Courts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var court = await _context.Courts.FindAsync(id);

            if (court == null)
                return NotFound();

            return Ok(court);
        }

        // POST: api/Courts
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Court court)
        {
            _context.Courts.Add(court);
            await _context.SaveChangesAsync();

            return Ok(court);
        }

        // PUT: api/Courts/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Court court)
        {
            if (id != court.Id)
                return BadRequest();

            _context.Entry(court).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(court);
        }

        // DELETE: api/Courts/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var court = await _context.Courts.FindAsync(id);

            if (court == null)
                return NotFound();

            _context.Courts.Remove(court);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
