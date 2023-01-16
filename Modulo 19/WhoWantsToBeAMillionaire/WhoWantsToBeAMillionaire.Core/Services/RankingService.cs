using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class RankingService : IRankingService
{
    private readonly IRankingRepository _rankingRepository;

    public RankingService(IRankingRepository rankingRepository)
        => _rankingRepository = rankingRepository;

    public List<RankingModel> GetTopFive()
    {
        var ranking = _rankingRepository.GetAll();

        return ranking
            .Select(s => new RankingModel(s.PlayerName, s.HelpCount, s.SkipCount, s.Award))
            .ToList();
    }

    //TODO: adicionar campo de data hora da partida
    public bool Create(string playerName, int helpCount, int skipCount, int award)
    {
        var ranking = new Ranking()
        {
            PlayerName = playerName,
            HelpCount = helpCount,
            SkipCount = skipCount,
            Award = award
        };

        return _rankingRepository.Create(ranking);
    }
}