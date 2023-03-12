using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class AwardService : IAwardService
{
    private readonly IAwardsRepository _awardRepository;

    public AwardService(IAwardsRepository awardRepository)
        => _awardRepository = awardRepository;

    public List<AwardsModel> GetAll()
    {
        var awards = _awardRepository.GetAll();

        return Parse(awards);
    }


    public static List<AwardsModel> Parse(List<Awards> awards)
        => awards
            .Select(award => new AwardsModel(award.Id, award.Correct, award.Stop, award.Wrong))
            .ToList();
}