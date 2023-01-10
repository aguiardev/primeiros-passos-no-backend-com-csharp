using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class AwardService : IAwardService
{
    private readonly IAwardRepository _awardRepository;

    public AwardService(IAwardRepository awardRepository)
        => _awardRepository = awardRepository;

    public List<AwardModel> GetAll()
    {
        var awards = _awardRepository.GetAll();

        return Parse(awards);
    }


    public static List<AwardModel> Parse(List<Award> awards)
        => awards
            .Select(award => new AwardModel(award.Id, award.Correct, award.Stop, award.Wrong))
            .ToList();
}