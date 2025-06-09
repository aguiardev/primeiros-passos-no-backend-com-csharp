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
        Constants.STOP,
        Constants.SKIP
    };
    private readonly IAwardService _awardService;
    private readonly IQuestionService _questionService;
    private readonly IRankingService _rankingService;

    public string PlayerName { get; private set; }
    public int CurrentAward { get; private set; }
    public int SkipCount { get; private set; }

    public event EventHandler<StartedArgs> OnStarted;
    public event EventHandler<NextQuestionArgs> OnNextQuestion;
    public event EventHandler<RightAnswerArgs> OnRightAnswer;
    public event EventHandler<GameOverArgs> OnGameOver;

    public GameService(
        IQuestionService questionService,
        IAwardService awardService,
        int skipCount,
        IRankingService rankingService)
    {
        _indexSelectedOption = -1;
        _questionIndex = _awardIndex = 0;
        _callStop = _callSkip = _isValidOption = false;
        _questionService = questionService;
        _awardService = awardService;

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

    private bool CanSkip()
    {
        if (SkipCount <= 0)
            return false;

        _callSkip = true;
        SkipCount--;
        return true;
    }

    public bool IsValidOption(string selectedOption, out string message)
    {
        message = string.Empty;
        
        _callSkip = _callStop = _isValidOption = false;

        if (!_validOption.Contains(selectedOption.Trim().ToUpperInvariant()))
        {
            message = "Opção inválida! Tente novamente.";
            return false;
        }

        _indexSelectedOption = -1;

        if (selectedOption.Trim() is
            Constants.OPTION_ONE or
            Constants.OPTION_TWO or
            Constants.OPTION_THREE or
            Constants.OPTION_FOUR)
        {
            _indexSelectedOption = int.Parse(selectedOption) - 1;
        }
        else
        {
            switch (selectedOption)
            {
                case Constants.SKIP:
                    if (!CanSkip())
                    {
                        message = "Você não pode mais pular!";
                        return false;
                    }
                    
                    break;
                case Constants.STOP:
                    _callStop = true;

                    break;
            }
        }

        _isValidOption = true;
        return true;
    }

    private bool IsRightAnswer(QuestionsModel question)
        => _isValidOption && question.Options[_indexSelectedOption].IsCorrect;

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

    private int GetCurrentQuestionNumber() => _awardIndex + 1;

    private bool TryGetNextQuestion(out QuestionsModel? question, out AwardsModel? award)
    {
        if (_awardIndex > _awards.Count)
        {
            question = null;
            award = null;
            return false;
        }

        question = _questions[_questionIndex];

        _questionIndex++;

        question.Number = GetCurrentQuestionNumber();
        award = _awards[_awardIndex];

        return true;
    }

    private bool IsFinalQuestion() => _awardIndex == _awards.Count - 1;

    public List<RankingsModel> GetTopFiveRanking() => _rankingService.GetTopFive();

    public void Start(string playerName)
    {
        LoadData();

        PlayerName = playerName;

        OnStarted?.Invoke(this, new StartedArgs(PlayerName));

        while (TryGetNextQuestion(out var question, out var award))
        {
            OnNextQuestion.Invoke(this, new NextQuestionArgs(
                PlayerName, CurrentAward, question, award, SkipCount));

            if (_callSkip)
                continue;

            if (_callStop)
            {
                CurrentAward = award.Stop;
                _gameOverReason = GameOverReason.Stopped;
                break;
            }

            if (IsRightAnswer(question))
            {
                CurrentAward = award.RightAnswer;
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
                CurrentAward = award.WrongAnswer;
                _gameOverReason = GameOverReason.Lost;
                break;
            }
        }

        _rankingService.Create(playerName, SkipCount, CurrentAward);
        OnGameOver?.Invoke(this, new GameOverArgs(_gameOverReason, CurrentAward));
    }
}