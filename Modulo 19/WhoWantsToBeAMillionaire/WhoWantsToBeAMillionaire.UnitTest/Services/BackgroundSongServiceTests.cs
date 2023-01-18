using WhoWantsToBeAMillionaire.Core.Services;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;

namespace WhoWantsToBeAMillionaire.UnitTest.Services
{
    public class BackgroundSongServiceTests
    {
        private readonly BackgroundSongService _backgroundService;
        private readonly Mock<ISoundPlayerService> _soundPlayer;

        public BackgroundSongServiceTests()
        {
            _soundPlayer = new();
            _backgroundService = new(_soundPlayer.Object);
        }

        [Fact(DisplayName = "Dada uma música, quando der play na música de abertura então deve ter sucesso.")]
        public void GivenASong_WhenPlaySongOpening_ThenShouldSuccess()
        {
            // arrange
            const string songName = "abertura.wav";

            // act
            _backgroundService.PlayOpening();

            // assert
            Assert.EndsWith(songName, _backgroundService.CurrentSoundLocation);
            _soundPlayer.Verify(v => v.PlayLooping(), Times.Once);
        }

        [Fact(DisplayName = "Dada uma música, quando der play na música de suspense então deve ter sucesso.")]
        public void GivenASong_WhenPlaySongThriller_ThenShouldSuccess()
        {
            // arrange
            const string songName = "suspense.wav";

            // act
            _backgroundService.PlayThriller();

            // assert
            Assert.EndsWith(songName, _backgroundService.CurrentSoundLocation);
            _soundPlayer.Verify(v => v.PlayLooping(), Times.Once);
        }

        [Fact(DisplayName = "Dada uma música, quando der play na música de seleção então deve ter sucesso.")]
        public void GivenASong_WhenPlaySongQuestionSelection_ThenShouldSuccess()
        {
            // arrange
            const string songName = "selecao-perguntas.wav";

            // act
            _backgroundService.PlayQuestionSelection(0);

            // assert
            Assert.EndsWith(songName, _backgroundService.CurrentSoundLocation);
            _soundPlayer.Verify(v => v.Play(), Times.Once);
        }

        [Fact(DisplayName = "Dada uma música, quando der play na música de encerramento então deve ter sucesso.")]
        public void GivenASong_WhenPlaySongGameOver_ThenShouldSuccess()
        {
            // arrange
            const string songName = "game-over.wav";

            // act
            _backgroundService.PlayGameOver(0);

            // assert
            Assert.EndsWith(songName, _backgroundService.CurrentSoundLocation);
            _soundPlayer.Verify(v => v.Play(), Times.Once);
        }
    }
}