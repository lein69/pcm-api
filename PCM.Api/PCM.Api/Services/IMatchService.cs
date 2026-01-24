using PCM.Api.Models;

namespace PCM.Api.Services
{
    public interface IMatchService
    {
        Task UpdateRanksAfterMatchAsync(Match match);
        Task UpdateChallengeScoreAsync(Match match);
    }
}