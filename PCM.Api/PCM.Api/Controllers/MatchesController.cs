using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Models;
using PCM.Api.Services;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMatchService _matchService;

        public MatchesController(ApplicationDbContext context, IMatchService matchService)
        {
            _context = context;
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Matches
                .Include(m => m.Challenge)
                .Include(m => m.Team1_Player1)
                .Include(m => m.Team2_Player1)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.Matches.FindAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            // Update ranks and challenge score
            await _matchService.UpdateRanksAfterMatchAsync(match);
            await _matchService.UpdateChallengeScoreAsync(match);

            return Ok(match);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Match match)
        {
            if (id != match.Id)
                return BadRequest();

            _context.Entry(match).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Update ranks and challenge score
            await _matchService.UpdateRanksAfterMatchAsync(match);
            await _matchService.UpdateChallengeScoreAsync(match);

            return Ok(match);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Matches.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Matches.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
