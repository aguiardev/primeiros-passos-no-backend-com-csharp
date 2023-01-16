using WhoWantsToBeAMillionaire.Core.Models;

namespace WhoWantsToBeAMillionaire.Core.Services.Interfaces;

public interface IRankingService
{
    List<RankingModel> GetTopFive();
    bool Create(string playerName, int helpCount, int skipCount, int award);
}