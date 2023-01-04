using WhoWantsToBeAMillionaire.App.Entities;
using WhoWantsToBeAMillionaire.App.Events;
using WhoWantsToBeAMillionaire.App.Events.Enums;

namespace WhoWantsToBeAMillionaire.App.Services;

public delegate void OnStartedHandler(object sender, StartedArgs args);
public delegate void OnNextQuestionHandler(object sender, NextQuestionArgs args);
public delegate void OnQuestionAsweredHandler(object sender, QuestionAsweredArgs args);

public class GameService
{
    private readonly List<Award> _awards;
    private readonly List<Question> _questions;
    private bool Stopped => string.Equals(
        OptionSelected, Constants.STOP, StringComparison.OrdinalIgnoreCase);

    public readonly string PlayerName;
    public int HelpCount { get; private set; }
    public int SkipCount { get; private set; }
    public decimal CurrentAward { get; private set; }
    public string OptionSelected { get; set; }

    public event OnStartedHandler? OnStarted;
    public event OnNextQuestionHandler? OnNextQuestion;
    public event OnQuestionAsweredHandler? OnQuestionAswered;

    public GameService(List<Question> questions, List<Award> awards, string playerName, int helpCount, int skipCount)
    {
        _questions = questions;
        _awards = awards;
        PlayerName = playerName;
        HelpCount = helpCount;
        SkipCount = skipCount;
        OptionSelected = string.Empty;
    }

    public void Start()
    {
        OnStarted?.Invoke(this, new StartedArgs(PlayerName));

        var questionNumber = 0;

        foreach (var question in _questions)
        {
            questionNumber++;

            var award = GetAward(questionNumber);

            OnNextQuestion?.Invoke(this, new NextQuestionArgs(
                questionNumber.ToString(),
                question,
                PlayerName,
                CurrentAward,
                SkipCount,
                HelpCount,
                award));

            if (Stopped)
            {
                UpdateCurrentAward(award.Stop);

                OnQuestionAswered?.Invoke(
                    this, QuestionAsweredArgs.CreateStopped(award.Stop));

                return;
            }

            if (!IsCorrect(question, OptionSelected!))
            {
                OnQuestionAswered?.Invoke(
                    this, new QuestionAsweredArgs(QuestionAswered.Wrong, award.Wrong));

                break;
            }

            UpdateCurrentAward(award.Correct);

            OnQuestionAswered?.Invoke(
                this, new QuestionAsweredArgs(QuestionAswered.Right));
        }
    }

    public Award GetAward(int awardNumber)
        => _awards.First(f => f.Number == awardNumber);

    public static bool IsValidOption(string? optionAlias)
        => !string.IsNullOrEmpty(optionAlias)
        && optionAlias.Trim().ToUpper() is
            Constants.OPTION_ONE or
            Constants.OPTION_TWO or
            Constants.OPTION_THREE or
            Constants.OPTION_FOUR or
            Constants.HELP or
            Constants.SKIP or
            Constants.STOP;

    bool IsCorrect(Question question, string optionAlias)
        => question.Options.Any(w => w.IsCorrect && w.Alias == optionAlias);

    public void UpdateCurrentAward(decimal currentAward) => CurrentAward = currentAward;

    public void GetHelp() => HelpCount--;

    public void GetSkip() => SkipCount--;

    public int GetQuestionCount() => _awards.Count;
}