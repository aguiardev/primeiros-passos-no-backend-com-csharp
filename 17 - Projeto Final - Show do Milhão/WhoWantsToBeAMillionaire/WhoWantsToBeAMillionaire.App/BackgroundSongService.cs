using System.Media;

namespace WhoWantsToBeAMillionaire.App
{
    public class BackgroundSongService
    {
        private readonly SoundPlayer _soundPlayer;

        private string BasePath
            => AppDomain.CurrentDomain.BaseDirectory + "\\BackgroundSongs\\{0}";

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