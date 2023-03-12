using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class QuestionsService : IQuestionService
{
    private readonly IOptionsRepository _optionsRepository;
    private readonly IQuestionsRepository _questionRepository;

    public QuestionsService(IOptionsRepository optionsRepository, IQuestionsRepository questionRepository)
    {
        _optionsRepository = optionsRepository;
        _questionRepository = questionRepository;
    }

    public List<QuestionsModel> GetAll()
    {
        var questions = _questionRepository.GetAll();
        var options = _optionsRepository.GetAll();

        return Parse(questions, options);
    }

    public static List<QuestionsModel> Parse(List<Questions> questions, List<Options> options)
    {
        List<QuestionsModel> questionsParsed = new();
        foreach (var question in questions)
        {
            var optionsParsed = options
                .Where(f => f.QuestionId == question.Id)
                .Select(s => new OptionsModel(s.Id, s.Description, s.Correct))
                .ToList();

            questionsParsed.Add(new(question.Id, question.Description, optionsParsed));
        }

        return questionsParsed;
    }
}
