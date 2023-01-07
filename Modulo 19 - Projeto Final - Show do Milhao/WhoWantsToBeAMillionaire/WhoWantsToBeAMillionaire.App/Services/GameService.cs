using WhoWantsToBeAMillionaire.App.Entities;
using WhoWantsToBeAMillionaire.App.Enums;
using WhoWantsToBeAMillionaire.App.Events;

namespace WhoWantsToBeAMillionaire.App.Services;

public delegate void OnStartedHandler(object sender, StartedArgs args);
public delegate void OnNextQuestionHandler(object sender, NextQuestionArgs args);
public delegate void OnRightAswerHandler(object sender, RightAswerArgs args);
public delegate void OnGameOverHandler(object sender, GameOverArgs args);

// TODO: criar partial RandomService
public class GameService
{
    private int _questionIndex;
    private int _awardIndex;
    private bool _callHelp;
    private bool _callSkip;
    private bool _callStop;
    private GameOverReason _gameOverReason;

    private readonly List<Award> _awards;
    private readonly List<Question> _questions;

    public readonly string PlayerName;
    public decimal CurrentAward { get; private set; }
    public int HelpCount { get; private set; }
    public int SkipCount { get; private set; }
    public string OptionNumberSelected { get; set; }

    public event OnStartedHandler? OnStarted;
    public event OnNextQuestionHandler? OnNextQuestion;
    public event OnRightAswerHandler? OnRightAswer;
    public event OnGameOverHandler? OnGameOver;

    public GameService(List<Question> questions, List<Award> awards, string playerName, int helpCount, int skipCount)
    {
        _questionIndex = 0;
        _awardIndex = 0;
        _callHelp = _callStop = _callSkip = false;

        Shuffle(questions);
        _questions = questions;
        _awards = awards;
        PlayerName = playerName;
        HelpCount = helpCount;
        SkipCount = skipCount;
        OptionNumberSelected = string.Empty;
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

    public bool IsValidOption(string optionAlias, bool callHelpBefore, out string message)
    {
        if (string.IsNullOrEmpty(optionAlias) &&
            optionAlias.Trim().ToUpper() is not
            Constants.OPTION_ONE or
            Constants.OPTION_TWO or
            Constants.OPTION_THREE or
            Constants.OPTION_FOUR)
        {
            message = "Opção inválida! Tente novamente.";
            return false;
        }

        _callSkip = _callStop = _callHelp = false;

        if (string.Equals(optionAlias, Constants.HELP, StringComparison.OrdinalIgnoreCase))
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

            message = string.Empty;
            return true;
        }

        if (string.Equals(optionAlias, Constants.SKIP, StringComparison.OrdinalIgnoreCase))
        {
            if (!CanSkip())
            {
                message = "Você não pode mais pular!";
                return false;
            }

            message = string.Empty;
            return true;
        }

        if (string.Equals(optionAlias, Constants.STOP, StringComparison.OrdinalIgnoreCase))
        {
            message = string.Empty;
            return _callStop = true;
        }

        message = string.Empty;
        return true;
    }

    private bool IsCorrect(Question question)
        => int.TryParse(OptionNumberSelected, out var optionNumberSelected)
        && question.Options.Any(w => w.IsCorrect && w.Number == optionNumberSelected);

    private void ResetOptionNumbers(List<Option> options)
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].Number = i + 1;
        }
    }

    private void RemoveRandomOption(List<Option> options)
    {
        var wrongOptions = options.Where(w => !w.IsCorrect);

        var removedElementIndex = new Random().Next(0, wrongOptions.Count());

        var removedElement = wrongOptions.ElementAt(removedElementIndex);

        options.Remove(removedElement);
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

    private bool NextQuestion(out Question? question, out Award? award)
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

    public void Start()
    {
        OnStarted?.Invoke(this, new StartedArgs(PlayerName));

        while (NextQuestion(out Question? question, out Award? award))
        {
            OnNextQuestion?.Invoke(this, new NextQuestionArgs(
                PlayerName, CurrentAward, question, award, SkipCount, HelpCount, _callHelp));

            if (_callHelp)
            {
                RemoveRandomOption(question.Options);
                ResetOptionNumbers(question.Options);

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
                OnRightAswer?.Invoke(this, new RightAswerArgs());
                
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