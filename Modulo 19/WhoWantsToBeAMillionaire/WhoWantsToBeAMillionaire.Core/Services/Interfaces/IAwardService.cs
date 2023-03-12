using WhoWantsToBeAMillionaire.Core.Models;

namespace WhoWantsToBeAMillionaire.Core.Services.Interfaces;

public interface IAwardService
{
    List<AwardsModel> GetAll();
}