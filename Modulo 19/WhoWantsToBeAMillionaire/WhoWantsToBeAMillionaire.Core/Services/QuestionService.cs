using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class QuestionService : IQuestionService
{
    private readonly IOptionsRepository _optionsRepository;
    private readonly IQuestionRepository _questionRepository;

    public QuestionService(IOptionsRepository optionsRepository, IQuestionRepository questionRepository)
    {
        _optionsRepository = optionsRepository;
        _questionRepository = questionRepository;
    }

    public List<QuestionModel> GetAll()
    {
        var questions = _questionRepository.GetAll();
        var options = _optionsRepository.GetAll();

        return Parse(questions, options);
    }

    public static List<QuestionModel> Parse(List<Question> questions, List<Options> options)
    {
        List<QuestionModel> questionsParsed = new();
        foreach (var question in questions)
        {
            var optionsParsed = options
                .Where(f => f.QuestionId == question.Id)
                .Select(s => new OptionsModel(s.Id, s.Description))
                .ToList();

            questionsParsed.Add(new(question.Id, question.Description, optionsParsed));
        }

        return questionsParsed;
    }
}
