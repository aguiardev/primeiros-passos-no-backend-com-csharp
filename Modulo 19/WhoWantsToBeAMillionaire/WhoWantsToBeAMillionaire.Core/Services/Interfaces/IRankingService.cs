using WhoWantsToBeAMillionaire.Data.Entities;

namespace WhoWantsToBeAMillionaire.Core.Services.Interfaces;

public interface IRankingService
{
    List<Ranking> TopFive();
    bool Create(string playerName, int helpCount, int skipCount, int award);
}