using WhoWantsToBeAMillionaire.Core.Models;

namespace WhoWantsToBeAMillionaire.Core.Services.Interfaces;

public interface IQuestionService
{
    List<QuestionsModel> GetAll();
}