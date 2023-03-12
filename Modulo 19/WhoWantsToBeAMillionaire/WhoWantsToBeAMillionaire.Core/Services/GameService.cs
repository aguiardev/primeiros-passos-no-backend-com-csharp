using WhoWantsToBeAMillionaire.Core.Enums;
using WhoWantsToBeAMillionaire.Core.Events;
using WhoWantsToBeAMillionaire.Core.Exceptions;
using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class GameService
{
    private int _indexSelectedOption;
    private int _questionIndex;
    private int _awardIndex;
    private bool _callHelp;
    private bool _callSkip;
    private bool _callStop;
    private bool _isValidOption;
    private GameOverReason _gameOverReason;
    private List<AwardsModel> _awards;
    private List<QuestionsModel> _questions;
    private readonly string[] _validOption = new string[]
    {
        Constants.OPTION_ONE,
        Constants.OPTION_TWO,
        Constants.OPTION_THREE,
        Constants.OPTION_FOUR,
        Constants.HELP,
        Constants.STOP,
        Constants.SKIP
    };
    private readonly IAwardService _awardService;
    private readonly IQuestionService _questionService;
    private readonly IRankingService _rankingService;

    public string PlayerName { get; private set; }
    public int CurrentAward { get; private set; }
    public int HelpCount { get; private set; }
    public int SkipCount { get; private set; }

    public event EventHandler<StartedArgs> OnStarted;
    public event EventHandler<NextQuestionArgs> OnNextQuestion;
    public event EventHandler<RightAnswerArgs> OnRightAnswer;
    public event EventHandler<GameOverArgs> OnGameOver;

    public GameService(
        IQuestionService questionService,
        IAwardService awardService,
        int helpCount,
        int skipCount,
        IRankingService rankingService)
    {
        _indexSelectedOption = -1;
        _questionIndex = _awardIndex = 0;
        _callHelp = _callStop = _callSkip = _isValidOption = false;
        _questionService = questionService;
        _awardService = awardService;

        HelpCount = helpCount;
        SkipCount = skipCount;
        _rankingService = rankingService;
    }

    private void LoadData()
    {
        _awards = _awardService.GetAll();
        if (!_awards.Any())
            throw new AwardListNotFoundException("A lista de prêmios está vazia.");

        _questions = _questionService.GetAll();
        if (!_questions.Any())
            throw new QuestionListNotFoundException("A lista de questões está vazia.");

        var invalidQuestions = _questions
            .Where(a => a.Options.Count < 4)
            .Select(s => s.Id);

        if (invalidQuestions.Any())
            throw new OptionListNotFoundException(
                $"Id das perguntas que estão com menos de 4 opções cadastradas: {string.Join(',', invalidQuestions)}");

        if (_questions.Count < _awards.Count)
            throw new LessQuestionThanAwardsException("Há menos perguntas do que prêmios cadastrados.");

        // TODO: verificar se a quantidade de alternativas corretas cadastradas estão corretas.

        Shuffle(_questions);
    }

    private bool CanHelp()
    {
        if (HelpCount <= 0)
            return false;

        _callHelp = true;
        HelpCount--;
        return true;
    }

    private bool CanSkip()
    {
        if (SkipCount <= 0)
            return false;

        _callSkip = true;
        SkipCount--;
        return true;
    }

    public bool IsValidOption(
        string selectedOptionNumber, bool callHelpBefore, out string message)
    {
        message = string.Empty;
        
        _callSkip = _callStop = _callHelp = _isValidOption = false;

        if (!_validOption.Contains(selectedOptionNumber.Trim().ToUpper()))
        {
            message = "Opção inválida! Tente novamente.";
            return false;
        }

        if (selectedOptionNumber.Trim().ToUpper() is
            Constants.OPTION_ONE or
            Constants.OPTION_TWO or
            Constants.OPTION_THREE or
            Constants.OPTION_FOUR)
        {
            _indexSelectedOption = int.Parse(selectedOptionNumber) - 1;
        }
        else
        {
            _indexSelectedOption = -1;
        }

        if (_indexSelectedOption == -1)
        {
            if (string.Equals(selectedOptionNumber, Constants.HELP, StringComparison.OrdinalIgnoreCase))
            {
                if (callHelpBefore)
                {
                    message = "Você só pode pedir ajuda uma vez por pergunta!";
                    return false;
                }

                if (!CanHelp())
                {
                    message = "Você não pode mais pedir ajuda!";
                    return false;
                }
            }
            else if (string.Equals(selectedOptionNumber, Constants.SKIP, StringComparison.OrdinalIgnoreCase))
            {
                if (!CanSkip())
                {
                    message = "Você não pode mais pular!";
                    return false;
                }
            }
            else if (string.Equals(selectedOptionNumber, Constants.STOP, StringComparison.OrdinalIgnoreCase))
            {
                _callStop = true;
            }
        }

        _isValidOption = true;
        return true;
    }

    private bool IsCorrect(QuestionsModel question)
        => _isValidOption && question.Options[_indexSelectedOption].IsCorrect;

    private void RemoveWrongOptionRandomly(List<OptionsModel> options)
    {
        var wrongOptions = options.Where(w => !w.IsCorrect);

        var removedElementIndex = new Random().Next(0, wrongOptions.Count());

        wrongOptions.ElementAt(removedElementIndex).Hidden = true;
    }

    private void Shuffle<T>(List<T> list)
    {
        // reference: https://www.dotnetperls.com/fisher-yates-shuffle
        var random = new Random();
        var n = list.Count;
        for (int i = 0; i < (n - 1); i++)
        {
            int r = i + random.Next(n - i);
            T t = list[r];
            list[r] = list[i];
            list[i] = t;
        }
    }

    private int GetQuestioNumber() => _awardIndex + 1;

    private bool NextQuestion(out QuestionsModel? question, out AwardsModel? award)
    {
        if (_awardIndex > _awards.Count)
        {
            question = null;
            award = null;
            return false;
        }

        if (_callHelp)
        {
            question = _questions[_questionIndex - 1];
        }
        else
        {
            question = _questions[_questionIndex];

            _questionIndex++;
        }

        question.Number = GetQuestioNumber();
        award = _awards[_awardIndex];

        return true;
    }

    private bool IsFinalQuestion() => _awardIndex == _awards.Count - 1;

    public List<RankingsModel> GetTopFiveRaknking()
        => _rankingService.GetTopFive();

    public void Start(string playerName)
    {
        LoadData();

        PlayerName = playerName;

        OnStarted?.Invoke(this, new StartedArgs(PlayerName));

        while (NextQuestion(out var question, out var award))
        {
            OnNextQuestion?.Invoke(this, new NextQuestionArgs(
                PlayerName, CurrentAward, question, award, SkipCount, HelpCount, _callHelp));

            if (_callHelp)
            {
                RemoveWrongOptionRandomly(question.Options);
                continue;
            }

            if (_callSkip)
                continue;

            if (_callStop)
            {
                CurrentAward = award.Stop;
                _gameOverReason = GameOverReason.Stopped;
                break;
            }

            if (IsCorrect(question))
            {
                CurrentAward = award.Correct;
                OnRightAnswer?.Invoke(this, new RightAnswerArgs());
                
                if (IsFinalQuestion())
                {
                    _gameOverReason = GameOverReason.Won;
                    break;
                }

                _awardIndex++;
            }
            else
            {
                CurrentAward = award.Wrong;
                _gameOverReason = GameOverReason.Lost;
                break;
            }
        }

        _rankingService.Create(playerName, HelpCount, SkipCount, CurrentAward);
        OnGameOver?.Invoke(this, new GameOverArgs(_gameOverReason, CurrentAward));
    }
}