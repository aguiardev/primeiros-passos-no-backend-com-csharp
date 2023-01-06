using System.Media;

namespace WhoWantsToBeAMillionaire.App.Services
{
    public class BackgroundSongService
    {
        private readonly SoundPlayer _soundPlayer;

        private static string BasePath
            => AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\{0}";

        public BackgroundSongService(SoundPlayer soundPlayer)
            => _soundPlayer = soundPlayer;

        public void PlayOpening()
        {
            _soundPlayer.SoundLocation = string.Format(BasePath, "abertura.wav");
            _soundPlayer.PlayLooping();
        }

        public void PlayThriller()
        {
            _soundPlayer.SoundLocation = string.Format(BasePath, "suspense.wav");
            _soundPlayer.PlayLooping();
        }

        public void PlayQuestionSelection()
        {
            _soundPlayer.SoundLocation = string.Format(BasePath, "selecao-perguntas.wav");
            _soundPlayer.Play();

            Thread.Sleep(TimeSpan.FromSeconds(3));
        }

        public void PlayGameOver()
        {
            _soundPlayer.SoundLocation = string.Format(BasePath, "abertura.wav");
            _soundPlayer.Play();
        }
    }
}