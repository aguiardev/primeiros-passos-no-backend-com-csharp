using WhoWantsToBeAMillionaire.Core.Enums;
using WhoWantsToBeAMillionaire.Core.Events;
using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public delegate void OnStartedHandler(object sender, StartedArgs args);
public delegate void OnNextQuestionHandler(object sender, NextQuestionArgs args);
public delegate void OnRightAswerHandler(object sender, RightAswerArgs args);
public delegate void OnGameOverHandler(object sender, GameOverArgs args);

// TODO: criar partial RandomService
public class GameService
{
    private int _indexSelectedOption;
    private int _questionIndex;
    private int _awardIndex;
    private bool _callHelp;
    private bool _callSkip;
    private bool _callStop;
    private GameOverReason _gameOverReason;
    private List<AwardModel> _awards;
    private List<QuestionModel> _questions;
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

    public string PlayerName { get; private set; }
    public decimal CurrentAward { get; private set; }
    public int HelpCount { get; private set; }
    public int SkipCount { get; private set; }

    public event OnStartedHandler? OnStarted;
    public event OnNextQuestionHandler? OnNextQuestion;
    public event OnRightAswerHandler? OnRightAnswer;
    public event OnGameOverHandler? OnGameOver;

    public GameService(
        IQuestionService questionService, IAwardService awardService, int helpCount, int skipCount)
    {
        _indexSelectedOption = -1;
        _questionIndex = _awardIndex = 0;
        _callHelp = _callStop = _callSkip = false;
        _questionService = questionService;
        _awardService = awardService;

        HelpCount = helpCount;
        SkipCount = skipCount;

        LoadData();
    }

    private void LoadData()
    {
        _awards = _awardService.GetAll();
        _questions = _questionService.GetAll();
        
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
        
        _callSkip = _callStop = _callHelp = false;

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
        
        return true;
    }

    private bool IsCorrect(QuestionModel question)
        => question.Options[_indexSelectedOption].IsCorrect;

    private void RemoveRandomOption(List<OptionsModel> options)
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

    private bool NextQuestion(out QuestionModel? question, out AwardModel? award)
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

    public void Start(string playerName)
    {
        PlayerName = playerName;

        OnStarted?.Invoke(this, new StartedArgs(PlayerName));

        while (NextQuestion(out var question, out var award))
        {
            OnNextQuestion?.Invoke(this, new NextQuestionArgs(
                PlayerName, CurrentAward, question, award, SkipCount, HelpCount, _callHelp));

            if (_callHelp)
            {
                RemoveRandomOption(question.Options);
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
                OnRightAnswer?.Invoke(this, new RightAswerArgs());
                
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

        OnGameOver?.Invoke(this, new GameOverArgs(_gameOverReason, CurrentAward));
    }
}