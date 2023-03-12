using WhoWantsToBeAMillionaire.Data.Entities;

namespace WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

public interface IQuestionsRepository
{
    List<Questions> GetAll();
}