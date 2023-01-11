using WhoWantsToBeAMillionaire.Core.Exceptions;
using WhoWantsToBeAMillionaire.Core.Models;
using WhoWantsToBeAMillionaire.Core.Services;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;
using WhoWantsToBeAMillionaire.Data.Entities;

namespace WhoWantsToBeAMillionaire.UnitTest.Services
{
    public class GameServiceTests
    {
        private GameService _gameService;
        private readonly Mock<IQuestionService> _questionService;
        private readonly Mock<IAwardService> _awardService;

        public GameServiceTests()
        {
            _questionService = new Mock<IQuestionService>();
            _awardService = new Mock<IAwardService>();
            
        }

        [Fact(DisplayName = "Dada uma lista de prêmios vazia, quando carregar os dados deve lançar uma exceção.")]
        public void GivenEmptyAwardList_WhenLoadData_ThenThrowsException()
        {
            // arrange
            const string playerName = "Silvio";
            var emptyAwardList = new List<AwardModel>();

            _awardService.Setup(s => s.GetAll()).Returns(emptyAwardList);

            _gameService = new GameService(
                _questionService.Object, _awardService.Object, 3, 3);

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
                new AwardModel(1, 1000m, 0m, 0m),
                new AwardModel(2, 2000m, 1000m, 500m),
            };

            _awardService.Setup(s => s.GetAll()).Returns(awardList);

            var emptyQuestionList = new List<QuestionModel>();
            
            _questionService.Setup(s => s.GetAll()).Returns(emptyQuestionList);

            _gameService = new GameService(
                _questionService.Object, _awardService.Object, 3, 3);

            // assert
            Assert.Throws<QuestionListNotFoundException>(() => _gameService.Start(playerName));
        }

        [Fact(DisplayName = "Dada alguma pergunta com menos de 4 opções, quando carregar os dados deve lançar uma exceção.")]
        public void GivenSomeQuestionWithLessThanFourOptions_WhenLoadData_ThenThrowsException()
        {
            // arrange
            const string playerName = "Silvio";
            var awardList = new List<AwardModel>()
            {
                new AwardModel(1, 1000m, 0m, 0m),
                new AwardModel(2, 2000m, 1000m, 500m),
            };

            _awardService.Setup(s => s.GetAll()).Returns(awardList);

            var emptyQuestionList = new List<QuestionModel>()
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

            _questionService.Setup(s => s.GetAll()).Returns(emptyQuestionList);

            _gameService = new GameService(
                _questionService.Object, _awardService.Object, 3, 3);

            // assert
            Assert.Throws<OptionListNotFoundException>(() => _gameService.Start(playerName));
        }

        [Fact(DisplayName = "Dada quantidade de perguntas menor do que a quantidade de prêmios, quando carregar os dados deve lançar uma exceção.")]
        public void GivenNumberOfQuestionsLessThanNumberOfAwards_WhenLoadData_ThenThrowsException()
        {
            // arrange
            const string playerName = "Silvio";
            var awardList = new List<AwardModel>()
            {
                new AwardModel(1, 1000m, 0m, 0m),
                new AwardModel(2, 2000m, 1000m, 500m),
            };

            _awardService.Setup(s => s.GetAll()).Returns(awardList);

            var emptyQuestionList = new List<QuestionModel>()
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

            _questionService.Setup(s => s.GetAll()).Returns(emptyQuestionList);

            _gameService = new GameService(
                _questionService.Object, _awardService.Object, 3, 3);

            // assert
            Assert.Throws<LessQuestionThanAwardsException>(() => _gameService.Start(playerName));
        }
    }
}