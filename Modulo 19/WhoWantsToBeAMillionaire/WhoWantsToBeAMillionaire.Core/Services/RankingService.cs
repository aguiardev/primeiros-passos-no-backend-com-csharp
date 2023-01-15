using WhoWantsToBeAMillionaire.Core.Services.Interfaces;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class RankingService : IRankingService
{
    private readonly IRankingRepository _rankingRepository;

    public RankingService(IRankingRepository rankingRepository)
        => _rankingRepository = rankingRepository;

    public List<Ranking> TopFive()
    {
        // TODO: obtém todos os registros. Depois ordena por:
        // Maior prêmio e menos quantidade de pedidos de ajuda e pulos
        // Então pega os 5 primeiros.

        throw new NotImplementedException();
    }

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