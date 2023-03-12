using WhoWantsToBeAMillionaire.Data.Entities;

namespace WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

public interface IAwardsRepository
{
    List<Awards> GetAll();
}