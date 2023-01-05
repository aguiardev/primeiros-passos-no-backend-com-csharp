using WhoWantsToBeAMillionaire.App.Entities;
using WhoWantsToBeAMillionaire.App.Events;
using WhoWantsToBeAMillionaire.App.Enums;
using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.App.Services;

public delegate void OnStartedHandler(object sender, StartedArgs args);
public delegate void OnNextQuestionHandler(object sender, NextQuestionArgs args);
public delegate void OnQuestionAsweredHandler(object sender, QuestionAsweredArgs args);

public class GameService
{
    private bool _callHelp;
    private bool _callSkip;
    private List<Question> _selectedQuestions;

    /// <summary>
    /// Count Selected Questions + Count Skipped Questions
    /// </summary>
    private readonly List<int> _exceptionQuestionsId;
    private readonly List<Award> _awards;
    private readonly List<Question> _questions;

    public readonly string PlayerName;
    public decimal CurrentAward { get; private set; }
    public int HelpCount { get; private set; }
    public int SkipCount { get; private set; }
    public string OptionSelected { get; set; }

    public event OnStartedHandler? OnStarted;
    public event OnNextQuestionHandler? OnNextQuestion;
    public event OnQuestionAsweredHandler? OnQuestionAswered;

    public GameService(List<Question> questions, List<Award> awards, string playerName, int helpCount, int skipCount)
    {
        _callHelp = false;
        _callSkip = false;
        _exceptionQuestionsId = new List<int>();
        _questions = questions;
        _awards = awards;
        PlayerName = playerName;
        HelpCount = helpCount;
        SkipCount = skipCount;
        OptionSelected = string.Empty;
    }

    private bool CanStop() => string.Equals(
        OptionSelected, Constants.STOP, StringComparison.OrdinalIgnoreCase);

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

    public Award GetAward(int questionNumber)
        => _awards.First(f => f.QuestionNumber == questionNumber);

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

        message = string.Empty;
        return true;
    }

    private bool IsCorrect(Question question, int optionAlias)
        => question.Options.Any(w => w.IsCorrect && w.Number == optionAlias);

    private void ResetNumber(List<Option> options)
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].Number = i + 1;
        }
    }

    private void RemoveRandomOption(List<Option> options)
    {
        var wrongOptions = options.Where(w => !w.IsCorrect).ToList();

        var removedElementIndex = new Random().Next(0, wrongOptions.Count);

        var removedElement = wrongOptions[removedElementIndex];

        options.Remove(removedElement);
    }

    private Question GetRandomQuestion() => GetRandomQuestion(1).First();

    private List<Question> GetRandomQuestion(int count)
    {
        var availableQuestions = _questions
            .Where(w => !_exceptionQuestionsId.Contains(w.Id)).ToList();

        var question = new List<Question>();
        var random = new Random();

        for (int i = 0; i < count; i++)
        {
            var elementIndex = random.Next(0, availableQuestions.Count);

            question.Add(availableQuestions[elementIndex]);
        }

        return question;
    }

    public void UpdateCurrentAward(decimal currentAward) => CurrentAward = currentAward;

    public int GetQuestionCount() => _awards.Count;

    public void Start()
    {
        OnStarted?.Invoke(this, new StartedArgs(PlayerName));

        var questionNumber = 0;

        _selectedQuestions = GetRandomQuestion(_awards.Count);
        _exceptionQuestionsId.AddRange(_selectedQuestions.Select(s => s.Id));

        foreach (var question in _selectedQuestions)
        {
            question.Number = ++questionNumber;

            var award = GetAward(questionNumber);

            OnNextQuestion?.Invoke(this, new NextQuestionArgs(
                question, PlayerName, CurrentAward, SkipCount, false, HelpCount, award));

            if (_callHelp)
            {
                RemoveRandomOption(question.Options);
                ResetNumber(question.Options);
                
                OnNextQuestion?.Invoke(this, new NextQuestionArgs(
                    question, PlayerName, CurrentAward, SkipCount, true, HelpCount, award));

                _callHelp = false;
            }

            if (_callSkip)
            {
                question.UpdateProps(GetRandomQuestion());

                OnNextQuestion?.Invoke(this, new NextQuestionArgs(
                    question, PlayerName, CurrentAward, SkipCount, true, HelpCount, award));

                _callSkip = false;
                break;
            }

            if (CanStop())
            {
                UpdateCurrentAward(award.Stop);

                OnQuestionAswered?.Invoke(
                    this, QuestionAsweredArgs.CreateStopped(award.Stop));

                return;
            }

            if (!IsCorrect(question, int.Parse(OptionSelected)))
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
}