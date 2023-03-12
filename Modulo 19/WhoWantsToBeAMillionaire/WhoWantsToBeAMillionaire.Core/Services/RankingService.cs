using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class RankingService : IRankingService
{
    private readonly IRankingsRepository _rankingRepository;

    public RankingService(IRankingsRepository rankingRepository)
        => _rankingRepository = rankingRepository;

    public List<RankingsModel> GetTopFive()
    {
        var ranking = _rankingRepository.GetAll();

        return ranking
            .Select(s => new RankingsModel(s.PlayerName, s.HelpCount, s.SkipCount, s.Award))
            .ToList();
    }

    //TODO: adicionar campo de data hora da partida
    public bool Create(string playerName, int helpCount, int skipCount, int award)
    {
        var ranking = new Rankings()
        {
            PlayerName = playerName,
            HelpCount = helpCount,
            SkipCount = skipCount,
            Award = award
        };

        return _rankingRepository.Create(ranking);
    }
}