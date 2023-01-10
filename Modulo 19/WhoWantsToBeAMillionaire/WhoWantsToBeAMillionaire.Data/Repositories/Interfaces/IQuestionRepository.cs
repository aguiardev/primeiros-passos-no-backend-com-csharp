using WhoWantsToBeAMillionaire.Data.Entities;

namespace WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

public interface IQuestionRepository
{
    List<Question> GetAll();
}