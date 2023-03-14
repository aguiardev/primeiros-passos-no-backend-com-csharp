using WhoWantsToBeAMillionaire.Core.Models;

namespace WhoWantsToBeAMillionaire.Core.Services.Interfaces;

public interface IRankingService
{
    List<RankingsModel> GetTopFive();
    bool Create(string playerName, int skipCount, int award);
}