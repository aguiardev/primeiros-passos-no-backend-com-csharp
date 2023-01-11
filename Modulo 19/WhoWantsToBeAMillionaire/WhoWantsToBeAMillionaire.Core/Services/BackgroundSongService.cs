using System.Media;

namespace WhoWantsToBeAMillionaire.Core.Services;

//TODO: criar encapsular classe SoundPlayer
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

    public void PlayQuestionSelection(int delay = 3)
    {
        _soundPlayer.SoundLocation = string.Format(BasePath, "selecao-perguntas.wav");
        _soundPlayer.Play();

        Thread.Sleep(TimeSpan.FromSeconds(delay));
    }

    public void PlayGameOver()
    {
        _soundPlayer.SoundLocation = string.Format(BasePath, "abertura.wav");
        _soundPlayer.Play();
    }
}