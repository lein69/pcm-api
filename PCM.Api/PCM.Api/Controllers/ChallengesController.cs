using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.Api.Data;
using PCM.Api.Models;
using PCM.Api.Enums;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallengesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChallengesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Challenges
                .Include(c => c.CreatedBy)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.Challenges.FindAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Challenge challenge)
        {
            challenge.CreatedDate = DateTime.Now;

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            return Ok(challenge);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Challenge challenge)
        {
            if (id != challenge.Id)
                return BadRequest();

            challenge.ModifiedDate = DateTime.Now;

            _context.Entry(challenge).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(challenge);
        }

        [HttpPost("{id}/join")]
        public async Task<IActionResult> JoinChallenge(int id, [FromBody] int memberId)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
                return NotFound("Challenge not found");

            if (challenge.Status != ChallengeStatus.Open)
                return BadRequest("Challenge is not open for registration");

            var existingParticipant = await _context.Participants
                .AnyAsync(p => p.ChallengeId == id && p.MemberId == memberId);

            if (existingParticipant)
                return BadRequest("Already joined this challenge");

            var participant = new Participant
            {
                ChallengeId = id,
                MemberId = memberId,
                EntryFeePaid = false,
                EntryFeeAmount = challenge.EntryFee,
                JoinedDate = DateTime.Now,
                Status = ParticipantStatus.Pending
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return Ok(participant);
        }

        [HttpPost("{id}/auto-divide-teams")]
        public async Task<IActionResult> AutoDivideTeams(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
                return NotFound("Challenge not found");

            if (challenge.GameMode != GameMode.TeamBattle)
                return BadRequest("Only TeamBattle challenges can auto-divide teams");

            var participants = await _context.Participants
                .Where(p => p.ChallengeId == id && p.Status == ParticipantStatus.Confirmed)
                .Include(p => p.Member)
                .OrderByDescending(p => p.Member.RankLevel)
                .ToListAsync();

            for (int i = 0; i < participants.Count; i++)
            {
                participants[i].Team = i % 2 == 0 ? TeamSide.TeamA : TeamSide.TeamB;
            }

            await _context.SaveChangesAsync();

            return Ok(participants);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Challenges.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Challenges.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
