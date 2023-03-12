using WhoWantsToBeAMillionaire.Data.Entities;

namespace WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

public interface IRankingsRepository
{
    List<Rankings> GetAll();
    bool Create(Rankings ranking);
}