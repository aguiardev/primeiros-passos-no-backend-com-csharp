using WhoWantsToBeAMillionaire.Core.Services.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class BackgroundSongService
{
    private readonly ISoundPlayerService _soundPlayer;

    public string? CurrentSoundLocation { get; private set; }

    private static string BasePath
        => AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\{0}";

    public BackgroundSongService(ISoundPlayerService soundPlayer)
        => _soundPlayer = soundPlayer;

    private void SetSoundLocation(string soundLocation)
        => _soundPlayer.SoundLocation = CurrentSoundLocation = soundLocation;

    public void PlayOpening()
    {
        SetSoundLocation(string.Format(BasePath, "abertura.wav"));
        _soundPlayer.PlayLooping();
    }

    public void PlayThriller()
    {
        SetSoundLocation(string.Format(BasePath, "suspense.wav"));
        _soundPlayer.PlayLooping();
    }

    public void PlayQuestionSelection(int delay = 3)
    {
        SetSoundLocation(string.Format(BasePath, "selecao-perguntas.wav"));
        _soundPlayer.Play();

        Thread.Sleep(TimeSpan.FromSeconds(delay));
    }

    public void PlayGameOver(int delay = 5)
    {
        SetSoundLocation(string.Format(BasePath, "game-over.wav"));
        _soundPlayer.Play();

        Thread.Sleep(TimeSpan.FromSeconds(delay));
    }
}