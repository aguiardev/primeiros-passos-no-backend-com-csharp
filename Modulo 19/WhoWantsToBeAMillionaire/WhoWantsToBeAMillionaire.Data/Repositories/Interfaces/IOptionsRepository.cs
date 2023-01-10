using WhoWantsToBeAMillionaire.Data.Entities;

namespace WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

public interface IOptionsRepository
{
    List<Options> GetAll();
}