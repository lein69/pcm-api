using PCM.Api.Data;
using PCM.Api.Enums;
using PCM.Api.Models;

namespace PCM.Api.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateRanksAfterMatchAsync(Match match)
        {
            if (!match.IsRanked || match.WinningSide == WinningSide.None)
                return;

            var winners = GetWinners(match);
            var losers = GetLosers(match);

            foreach (var winner in winners)
            {
                winner.RankLevel += 0.1;
                winner.WinMatches++;
                winner.TotalMatches++;
                winner.ModifiedDate = DateTime.Now;
            }

            foreach (var loser in losers)
            {
                loser.RankLevel = Math.Max(0, loser.RankLevel - 0.1);
                loser.TotalMatches++;
                loser.ModifiedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateChallengeScoreAsync(Match match)
        {
            if (match.ChallengeId == null || match.WinningSide == WinningSide.None)
                return;

            var challenge = await _context.Challenges.FindAsync(match.ChallengeId);
            if (challenge == null || challenge.Status != ChallengeStatus.Ongoing)
                return;

            if (challenge.GameMode == GameMode.TeamBattle)
            {
                if (match.WinningSide == WinningSide.Team1)
                    challenge.CurrentScore_TeamA++;
                else if (match.WinningSide == WinningSide.Team2)
                    challenge.CurrentScore_TeamB++;

                if (challenge.CurrentScore_TeamA >= challenge.Config_TargetWins.GetValueOrDefault())
                {
                    challenge.Status = ChallengeStatus.Finished;
                    // TODO: Distribute prize
                }
                else if (challenge.CurrentScore_TeamB >= challenge.Config_TargetWins.GetValueOrDefault())
                {
                    challenge.Status = ChallengeStatus.Finished;
                    // TODO: Distribute prize
                }
            }

            challenge.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        private List<Member> GetWinners(Match match)
        {
            var winners = new List<Member>();
            if (match.WinningSide == WinningSide.Team1)
            {
                winners.Add(match.Team1_Player1);
                if (match.MatchFormat == MatchFormat.Doubles && match.Team1_Player2 != null)
                    winners.Add(match.Team1_Player2);
            }
            else if (match.WinningSide == WinningSide.Team2)
            {
                winners.Add(match.Team2_Player1);
                if (match.MatchFormat == MatchFormat.Doubles && match.Team2_Player2 != null)
                    winners.Add(match.Team2_Player2);
            }
            return winners.Where(m => m != null).ToList();
        }

        private List<Member> GetLosers(Match match)
        {
            var losers = new List<Member>();
            if (match.WinningSide == WinningSide.Team1)
            {
                losers.Add(match.Team2_Player1);
                if (match.MatchFormat == MatchFormat.Doubles && match.Team2_Player2 != null)
                    losers.Add(match.Team2_Player2);
            }
            else if (match.WinningSide == WinningSide.Team2)
            {
                losers.Add(match.Team1_Player1);
                if (match.MatchFormat == MatchFormat.Doubles && match.Team1_Player2 != null)
                    losers.Add(match.Team1_Player2);
            }
            return losers.Where(m => m != null).ToList();
        }
    }
}