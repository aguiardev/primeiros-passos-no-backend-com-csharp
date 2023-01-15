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
        const int skipCount = 3;

        var emptyAwardList = new List<AwardModel>();

        _awardService.Setup(s => s.GetAll()).Returns(emptyAwardList);

        _gameService = new GameService(
            _questionService.Object,
            _awardService.Object,
            helpCount, 
            skipCount,
            _rankingService.Object);

        // assert
        Assert.Throws<AwardListNotFoundException>(() => _gameService.Start(playerName));
    }

    [Fact(DisplayName = "Dada uma lista de perguntas vazia, quando carregar os dados deve lançar uma exceção.")]
    public void GivenEmptyQuestionList_WhenLoadData_ThenThrowsException()
    {
        // arrange
        const string playerName = "Silvio";
        var awardList = new List<AwardModel>()
        {
            new AwardModel(1, 1000, 0, 0),
            new AwardModel(2, 2000, 1000, 500),
        };

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var emptyQuestionList = new List<QuestionModel>();

        _questionService.Setup(s => s.GetAll()).Returns(emptyQuestionList);

        _gameService = new GameService(
            _questionService.Object, _awardService.Object, 3, 3, _rankingService.Object);

        // assert
        Assert.Throws<QuestionListNotFoundException>(() => _gameService.Start(playerName));
    }

    [Fact(DisplayName = "Dada alguma pergunta com menos de 4 opções, quando carregar os dados deve lançar uma exceção.")]
    public void GivenSomeQuestionWithLessThanFourOptions_WhenLoadData_ThenThrowsException()
    {
        // arrange
        const string playerName = "Silvio";
        const int helpCount = 3;
        const int skipCount = 3;

        var awardList = new List<AwardModel>()
        {
            new AwardModel(1, 1000, 0, 0),
            new AwardModel(2, 2000, 1000, 500),
        };

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionModel>()
        {
            new QuestionModel(
                1,
                "Qual a capital da China?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Kyoto", false),
                    new OptionsModel(2, "Pequim", true),
                    new OptionsModel(3, "Taiwan", false),
                    new OptionsModel(4, "Brasilia", false)
                }),
            new QuestionModel(
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
            skipCount,
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
        const int skipCount = 3;

        var awardList = new List<AwardModel>()
        {
            new AwardModel(1, 1000, 0, 0),
            new AwardModel(2, 2000, 1000, 500),
        };

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionModel>()
        {
            new QuestionModel(
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
            skipCount,
            _rankingService.Object);

        // assert
        Assert.Throws<LessQuestionThanAwardsException>(() => _gameService.Start(playerName));
    }

    [Fact(DisplayName = "Dado todas as perguntas respondidas corretamente, quando o jogo terminar deverá ganhar o jogo.")]
    public void GivenAllTheQuestionsAnsweredCorrectly_WhenGameIsOver_ShouldWinTheGame()
    {
        // arrange
        const string playerName = "Silvio";
        const int helpCount = 3;
        const int skipCount = 3;

        var awardList = new List<AwardModel>()
        {
            new AwardModel(1, 1000, 0, 0),
            new AwardModel(2, 2000, 1000, 500),
            new AwardModel(3, 3000, 2000, 1000),
            new AwardModel(4, 4000, 3000, 1500)
        };

        var expectedAward = awardList.Max(m => m.Correct);

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionModel>()
        {
            new QuestionModel(
                1,
                "Qual a capital da China?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Kyoto", false),
                    new OptionsModel(2, "Pequim", true),
                    new OptionsModel(3, "Taiwan", false),
                    new OptionsModel(4, "Brasilia", false)
                }),
            new QuestionModel(
                2,
                "Qual o nome do terceiro planeta do sistema solar?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Marte", false),
                    new OptionsModel(2, "Plutão", false),
                    new OptionsModel(3, "Terra", true),
                    new OptionsModel(4, "Mercúrio", false)
                }),
            new QuestionModel(
                3,
                "Qual o ponto mais alto do Brasil?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Pico da Bandeira", false),
                    new OptionsModel(2, "Pico do Calçado", false),
                    new OptionsModel(3, "Pico 31 de Março", false),
                    new OptionsModel(4, "Pico da Neblina", true)
                }),
            new QuestionModel(
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
            helpCount,
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

            isValidOption = _gameService.IsValidOption(correctOption, args.CallHelp, out _);
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
        Assert.Equal(helpCount, _gameService.HelpCount);
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
        const int helpCount = 3;
        const int skipCount = 3;

        var awardList = new List<AwardModel>()
        {
            new AwardModel(1, 1000, 0, 0),
            new AwardModel(2, 2000, 1000, 500),
            new AwardModel(3, 3000, 2000, 1000),
            new AwardModel(4, 4000, 3000, 1500)
        };

        var expectedAward = 0m;

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionModel>()
        {
            new QuestionModel(
                1,
                "Qual a capital da China?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Kyoto", false),
                    new OptionsModel(2, "Pequim", true),
                    new OptionsModel(3, "Taiwan", false),
                    new OptionsModel(4, "Brasilia", false)
                }),
            new QuestionModel(
                2,
                "Qual o nome do terceiro planeta do sistema solar?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Marte", false),
                    new OptionsModel(2, "Plutão", false),
                    new OptionsModel(3, "Terra", true),
                    new OptionsModel(4, "Mercúrio", false)
                }),
            new QuestionModel(
                3,
                "Qual o ponto mais alto do Brasil?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Pico da Bandeira", false),
                    new OptionsModel(2, "Pico do Calçado", false),
                    new OptionsModel(3, "Pico 31 de Março", false),
                    new OptionsModel(4, "Pico da Neblina", true)
                }),
            new QuestionModel(
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
            helpCount,
            skipCount,
            _rankingService.Object);

        var isValidOption = false;

        _gameService.OnNextQuestion += (sender, args) =>
        {
            isValidOption = _gameService.IsValidOption(invalidOption, args.CallHelp, out _);
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
        Assert.Equal(helpCount, _gameService.HelpCount);
        Assert.Equal(expectedAward, _gameService.CurrentAward);
        Assert.False(isValidOption);
    }

    //TODO: fazer teste pulando a primeira, segunda, terceira e última pergunta.
    //[Fact(Skip = "Incompleto", DisplayName = "Dado o início do jogo, quando o jogador pedir ajuda uma opção errada deverá ser removida.")]
    [Fact(DisplayName = "Dado o início do jogo, quando o jogador pedir ajuda uma opção errada deverá ser removida.")]
    public void GivenTheBeginningOfTheGame_WhenThePlayerCallForHelp_AWrongOptionShouldBeRemoved()
    {
        // arrange
        const string playerName = "Silvio";
        const int helpCount = 3;
        const int skipCount = 3;
        const int helpCountExpected = 2;

        var awardList = new List<AwardModel>()
        {
            new AwardModel(1, 1000, 0, 0),
            //new AwardModel(2, 2000m, 1000m, 500m),
            //new AwardModel(3, 3000m, 2000m, 1000m),
            //new AwardModel(4, 4000m, 3000m, 1500m)
        };

        var expectedAward = awardList.Max(m => m.Correct);

        _awardService.Setup(s => s.GetAll()).Returns(awardList);

        var questionList = new List<QuestionModel>()
        {
            new QuestionModel(
                1,
                "Qual a capital da China?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Kyoto", false),
                    new OptionsModel(2, "Pequim", true),
                    new OptionsModel(3, "Taiwan", false),
                    new OptionsModel(4, "Brasilia", false)
                }),
            new QuestionModel(
                2,
                "Qual o nome do terceiro planeta do sistema solar?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Marte", false),
                    new OptionsModel(2, "Plutão", false),
                    new OptionsModel(3, "Terra", true),
                    new OptionsModel(4, "Mercúrio", false)
                }),
            new QuestionModel(
                3,
                "Qual o ponto mais alto do Brasil?",
                new List<OptionsModel>()
                {
                    new OptionsModel(1, "Pico da Bandeira", false),
                    new OptionsModel(2, "Pico do Calçado", false),
                    new OptionsModel(3, "Pico 31 de Março", false),
                    new OptionsModel(4, "Pico da Neblina", true)
                }),
            new QuestionModel(
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
            helpCount,
            skipCount,
            _rankingService.Object);

        var onNextQuestionRaisesCountExpected = awardList.Count + 1;
        var onNextQuestionRaisesCountActual = 0;

        QuestionModel question = null, question2 = null;

        _gameService.OnNextQuestion += (sender, args) =>
        {
            onNextQuestionRaisesCountActual++;

            if (onNextQuestionRaisesCountActual == 2)
            {
                question2 = args.Question;

                //TODO: encapsular
                var correctOption = questionList
                    .First(w => w.Id == args.Question.Id)
                    .Options
                    .First(w => w.IsCorrect)
                    .Number
                    .ToString();

                _ = _gameService.IsValidOption(correctOption, args.CallHelp, out _);
            }
            else
            {
                question = args.Question;

                _ = _gameService.IsValidOption(Constants.HELP, args.CallHelp, out _);
            }
        };

        // act
        var receivedEvent = Assert.Raises<GameOverArgs>(
            a => _gameService.OnGameOver += a,
            a => _gameService.OnGameOver -= a,
            () => _gameService.Start(playerName));

        // assert
        Assert.NotNull(receivedEvent);
        Assert.Equal(GameOverReason.Won, receivedEvent.Arguments.GameOverReason);
        Assert.Equal(helpCountExpected, _gameService.HelpCount);
        Assert.Equal(skipCount, _gameService.SkipCount);
        Assert.Equal(expectedAward, _gameService.CurrentAward);
        Assert.Equal(onNextQuestionRaisesCountExpected, onNextQuestionRaisesCountActual);

        //Ser a mesma pergunta anterior
        Assert.Equal(question.Id, question2.Id);
        //Continuar com apenas uma opção correta
        Assert.True(question2.Options.Count(c => c.IsCorrect) == 1);
        //Ter somente uma opção oculta
        Assert.True(question2.Options.Count(c => c.Hidden) == 1);
    }
}