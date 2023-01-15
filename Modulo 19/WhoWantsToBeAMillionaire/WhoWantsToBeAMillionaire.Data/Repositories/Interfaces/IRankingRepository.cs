using WhoWantsToBeAMillionaire.Data.Entities;

namespace WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

public interface IRankingRepository
{
    List<Ranking> GetAll();
    bool Create(Ranking ranking);
}