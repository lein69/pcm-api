using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Models;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.News.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            news.CreatedDate = DateTime.UtcNow;

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return Ok(news);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.News.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.News.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
