using WhoWantsToBeAMillionaire.Core;
using WhoWantsToBeAMillionaire.Core.Enums;
using WhoWantsToBeAMillionaire.Core.Events;
using WhoWantsToBeAMillionaire.Core.Exceptions;
using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;

namespace WhoWantsToBeAMillionaire.UnitTest.Services;

public class GameServiceTests
{
    private GameService _gameService;
    private readonly Mock<IQuestionService> _questionService;
    private readonly Mock<IAwardService> _awardService;
    private readonly Mock<IRankingService> _rankingService;

    public GameServiceTests()
    {
        _questionService = new Mock<IQuestionService>();
        _awardService = new Mock<IAwardService>();
        _rankingService = new Mock<IRankingService>();
    }

    [Fact(DisplayName = "Dada uma lista de prêmios vazia, quando carregar os dados deve lançar uma exceção.")]
    public void GivenEmptyAwardList_WhenLoadData_ThenThrowsException()
    {
        // arrange
        const string playerName = "Silvio";
        const int helpCount = 3;

        var emptyAwardList = new List<AwardsModel>();

        _awardService.Setup(s => s.GetAll()).Returns(emptyAwardList);

        _gameService = new GameService(
            _questionService.Object,
            _awardService.Object,
            helpCount, 
            _rankingService.Object);

        // assert
        Assert.Throws<AwardListNotFoundException>(() => _gameService.Start(playerName));
    }

    [Fact(DisplayName = "Dada uma lista de perguntas vazia, quando carregar os dados deve lançar uma exceção.")]
    public void GivenEmptyQuestionList_WhenLoadData_ThenThrowsException()
    {
        // arrange
        const string playerName = "Silvio";
        var awardList = new List<AwardsModel>()
        {
            new AwardsModel(1, 1000, 0, 0),
            new AwardsModel(2, 2000, 1000, 500),
        };

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var emptyQuestionList = new List<QuestionsModel>();

        _questionService.Setup(s => s.GetAll()).Returns(emptyQuestionList);

        _gameService = new GameService(
            _questionService.Object, _awardService.Object, 3, _rankingService.Object);

        // assert
        Assert.Throws<QuestionListNotFoundException>(() => _gameService.Start(playerName));
    }

    [Fact(DisplayName = "Dada alguma pergunta com menos de 4 opções, quando carregar os dados deve lançar uma exceção.")]
    public void GivenSomeQuestionWithLessThanFourOptions_WhenLoadData_ThenThrowsException()
    {
        // arrange
        const string playerName = "Silvio";
        const int helpCount = 3;

        var awardList = new List<AwardsModel>()
        {
            new AwardsModel(1, 1000, 0, 0),
            new AwardsModel(2, 2000, 1000, 500),
        };

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionsModel>()
        {
            new QuestionsModel(
                1,
                "Qual a capital da China?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Kyoto", false),
                    new OptionsModel(2, "Pequim", true),
                    new OptionsModel(3, "Taiwan", false),
                    new OptionsModel(4, "Brasilia", false)
                }),
            new QuestionsModel(
                2,
                "Qual o nome do terceiro planeta do sistema solar?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Marte", false),
                    new OptionsModel(2, "Plutão", false),
                    new OptionsModel(3, "Terra", true),
                })
        };

        _questionService.Setup(s => s.GetAll()).Returns(questionList);

        _gameService = new GameService(
            _questionService.Object,
            _awardService.Object,
            helpCount,
            _rankingService.Object);

        // assert
        Assert.Throws<OptionListNotFoundException>(() => _gameService.Start(playerName));
    }

    [Fact(DisplayName = "Dada quantidade de perguntas menor do que a quantidade de prêmios, quando carregar os dados deve lançar uma exceção.")]
    public void GivenNumberOfQuestionsLessThanNumberOfAwards_WhenLoadData_ThenThrowsException()
    {
        // arrange
        const string playerName = "Silvio";
        const int helpCount = 3;

        var awardList = new List<AwardsModel>()
        {
            new AwardsModel(1, 1000, 0, 0),
            new AwardsModel(2, 2000, 1000, 500),
        };

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionsModel>()
        {
            new QuestionsModel(
                1,
                "Qual a capital da China?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Kyoto", false),
                    new OptionsModel(2, "Pequim", true),
                    new OptionsModel(3, "Taiwan", false),
                    new OptionsModel(4, "Brasilia", false)
                })
        };

        _questionService.Setup(s => s.GetAll()).Returns(questionList);

        _gameService = new GameService(
            _questionService.Object,
            _awardService.Object,
            helpCount,
            _rankingService.Object);

        // assert
        Assert.Throws<LessQuestionThanAwardsException>(() => _gameService.Start(playerName));
    }

    [Fact(DisplayName = "Dado todas as perguntas respondidas corretamente, quando o jogo terminar deverá ganhar o jogo.")]
    public void GivenAllTheQuestionsAnsweredCorrectly_WhenGameIsOver_ShouldWinTheGame()
    {
        // arrange
        const string playerName = "Silvio";
        const int skipCount = 3;

        var awardList = new List<AwardsModel>()
        {
            new AwardsModel(1, 1000, 0, 0),
            new AwardsModel(2, 2000, 1000, 500),
            new AwardsModel(3, 3000, 2000, 1000),
            new AwardsModel(4, 4000, 3000, 1500)
        };

        var expectedAward = awardList.Max(m => m.Correct);

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionsModel>()
        {
            new QuestionsModel(
                1,
                "Qual a capital da China?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Kyoto", false),
                    new OptionsModel(2, "Pequim", true),
                    new OptionsModel(3, "Taiwan", false),
                    new OptionsModel(4, "Brasilia", false)
                }),
            new QuestionsModel(
                2,
                "Qual o nome do terceiro planeta do sistema solar?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Marte", false),
                    new OptionsModel(2, "Plutão", false),
                    new OptionsModel(3, "Terra", true),
                    new OptionsModel(4, "Mercúrio", false)
                }),
            new QuestionsModel(
                3,
                "Qual o ponto mais alto do Brasil?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Pico da Bandeira", false),
                    new OptionsModel(2, "Pico do Calçado", false),
                    new OptionsModel(3, "Pico 31 de Março", false),
                    new OptionsModel(4, "Pico da Neblina", true)
                }),
            new QuestionsModel(
                4,
                "Qual o dia da independência nos EUA?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "23 de Março", false),
                    new OptionsModel(2, "4 de Julho", true),
                    new OptionsModel(3, "7 de Setembro", false),
                    new OptionsModel(4, "15 de Novembro", false)
                })
        };

        _questionService.Setup(s => s.GetAll()).Returns(questionList);
        
        _gameService = new GameService(
            _questionService.Object,
            _awardService.Object,
            skipCount,
            _rankingService.Object);

        var isValidOption = false;

        _gameService.OnNextQuestion += (sender, args) =>
        {
            var correctOption = questionList
                .First(w => w.Id == args.Question.Id)
                .Options
                .First(w => w.IsCorrect)
                .Number
                .ToString();

            isValidOption = _gameService.IsValidOption(correctOption, out _);
        };

        // act
        var receivedEvent = Assert.Raises<GameOverArgs>(
            a => _gameService.OnGameOver += a,
            a => _gameService.OnGameOver -= a,
            () => _gameService.Start(playerName));

        // assert
        Assert.NotNull(receivedEvent);
        Assert.Equal(GameOverReason.Won, receivedEvent.Arguments.GameOverReason);
        Assert.Equal(skipCount, _gameService.SkipCount);
        Assert.Equal(expectedAward, _gameService.CurrentAward);
        Assert.True(isValidOption);
    }

    [Theory(DisplayName = "Dada uma opção inválida, quando jogador responder deverá perder o jogo.")]
    [InlineData("0")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("-189")]
    [InlineData("350")]
    [InlineData("99999999")]
    [InlineData("ABC")]
    [InlineData("1234")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    [InlineData("X")]
    [InlineData("Y")]
    [InlineData("Z")]
    [InlineData("!")]
    [InlineData("@")]
    [InlineData("#")]
    [InlineData("$")]
    public void GivenInvalidOption_WhenPlayerAnswers_ShouldLoseTheGame(string invalidOption)
    {
        // arrange
        const string playerName = "Silvio";
        const int skipCount = 3;

        var awardList = new List<AwardsModel>()
        {
            new AwardsModel(1, 1000, 0, 0),
            new AwardsModel(2, 2000, 1000, 500),
            new AwardsModel(3, 3000, 2000, 1000),
            new AwardsModel(4, 4000, 3000, 1500)
        };

        var expectedAward = 0m;

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionsModel>()
        {
            new QuestionsModel(
                1,
                "Qual a capital da China?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Kyoto", false),
                    new OptionsModel(2, "Pequim", true),
                    new OptionsModel(3, "Taiwan", false),
                    new OptionsModel(4, "Brasilia", false)
                }),
            new QuestionsModel(
                2,
                "Qual o nome do terceiro planeta do sistema solar?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Marte", false),
                    new OptionsModel(2, "Plutão", false),
                    new OptionsModel(3, "Terra", true),
                    new OptionsModel(4, "Mercúrio", false)
                }),
            new QuestionsModel(
                3,
                "Qual o ponto mais alto do Brasil?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Pico da Bandeira", false),
                    new OptionsModel(2, "Pico do Calçado", false),
                    new OptionsModel(3, "Pico 31 de Março", false),
                    new OptionsModel(4, "Pico da Neblina", true)
                }),
            new QuestionsModel(
                4,
                "Qual o dia da independência nos EUA?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "23 de Março", false),
                    new OptionsModel(2, "4 de Julho", true),
                    new OptionsModel(3, "7 de Setembro", false),
                    new OptionsModel(4, "15 de Novembro", false)
                })
        };

        _questionService.Setup(s => s.GetAll()).Returns(questionList);

        _gameService = new GameService(
            _questionService.Object,
            _awardService.Object,
            skipCount,
            _rankingService.Object);

        var isValidOption = false;

        _gameService.OnNextQuestion += (sender, args) =>
        {
            isValidOption = _gameService.IsValidOption(invalidOption, out _);
        };

        // act
        var receivedEvent = Assert.Raises<GameOverArgs>(
            a => _gameService.OnGameOver += a,
            a => _gameService.OnGameOver -= a,
            () => _gameService.Start(playerName));

        // assert
        Assert.NotNull(receivedEvent);
        Assert.Equal(GameOverReason.Lost, receivedEvent.Arguments.GameOverReason);
        Assert.Equal(skipCount, _gameService.SkipCount);
        Assert.Equal(expectedAward, _gameService.CurrentAward);
        Assert.False(isValidOption);
    }
}