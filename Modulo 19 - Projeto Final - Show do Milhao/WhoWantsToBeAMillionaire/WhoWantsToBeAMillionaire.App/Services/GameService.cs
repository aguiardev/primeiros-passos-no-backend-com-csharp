using WhoWantsToBeAMillionaire.App.Entities;
using WhoWantsToBeAMillionaire.App.Events;
using WhoWantsToBeAMillionaire.App.Enums;
using System.Collections.Generic;
using System;

namespace WhoWantsToBeAMillionaire.App.Services;

public delegate void OnStartedHandler(object sender, StartedArgs args);
public delegate void OnNextQuestionHandler(object sender, NextQuestionArgs args);
public delegate void OnQuestionAsweredHandler(object sender, QuestionAsweredArgs args);

// TODO: criar partial RandomService
public class GameService
{
    private int _questionIndex;
    private int _awardIndex;
    private bool _callHelp;
    private bool _callSkip;
    private bool _callStop;

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
        _questionIndex = 0;
        _awardIndex = 0;
        _callHelp = _callStop = _callSkip = false;
        _questions = questions;
        _awards = awards;
        PlayerName = playerName;
        HelpCount = helpCount;
        SkipCount = skipCount;
        OptionSelected = string.Empty;
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

    private bool IsCorrect(Question question, int optionAlias)
        => question.Options.Any(w => w.IsCorrect && w.Number == optionAlias);

    private void ResetOptionNumbers(List<Option> options)
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

    private List<Question> GetRandomQuestions()
    {
        //TODO: esse método deve embaralhar as questões

        //var availableQuestions = _questions
        //    .Where(w => !_exceptionQuestionsId.Contains(w.Id)).ToList();

        //var question = new List<Question>();
        //var random = new Random();

        //for (int i = 0; i < count; i++)
        //{
        //    var elementIndex = random.Next(0, availableQuestions.Count);

        //    //availableQuestions.ElementAt(elementIndex);

        //    question.Add(availableQuestions[elementIndex]);
        //}

        return null;
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

    public void Start()
    {
        OnStarted?.Invoke(this, new StartedArgs(PlayerName));

        //TODO: embaralhar ordem das questões

        while (NextQuestion(out Question? question, out Award? award))
        {
            OnNextQuestion?.Invoke(this, new NextQuestionArgs(
                PlayerName, CurrentAward, question, award, SkipCount, HelpCount, _callHelp));

            // TODO: Transformar _callHelp, _callSkip e _callStop em tipo enumerado

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

                OnQuestionAswered?.Invoke(
                    this, QuestionAsweredArgs.CreateStopped(award.Stop));

                break;
            }

            if (int.TryParse(OptionSelected, out var optionConverted) &&
                !IsCorrect(question, optionConverted))
            {
                OnQuestionAswered?.Invoke(
                    this, new QuestionAsweredArgs(QuestionAswered.Wrong, award.Wrong));

                break;
            }

            _awardIndex++;

            CurrentAward = award.Correct;

            OnQuestionAswered?.Invoke(
                this, new QuestionAsweredArgs(QuestionAswered.Right));
        }

        // Evento OnGameOver
        // Parabéns, você ganhou 1 milhão de reais!
    }
}