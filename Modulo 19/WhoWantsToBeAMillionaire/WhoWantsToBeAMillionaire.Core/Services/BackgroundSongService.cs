using WhoWantsToBeAMillionaire.Core.Services.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

public class BackgroundSongService
{
    private const string SONG_OPENNING = "abertura.wav";
    private const string SONG_THRILLER = "suspense.wav";
    private const string QUESTION_SELECTION = "selecao-perguntas.wav";
    private const string SONG_GAME_OVER = "game-over.wav";
    private readonly ISoundPlayerService _soundPlayer;

    public string? CurrentSoundLocation { get; private set; }

    private static string BasePath
        => AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\{0}";

    public BackgroundSongService(ISoundPlayerService soundPlayer)
        => _soundPlayer = soundPlayer;

    private void SetSoundLocation(string soundLocation)
        => _soundPlayer.SoundLocation = CurrentSoundLocation = string.Format(BasePath, soundLocation);

    public void PlayOpenning()
    {
        SetSoundLocation(SONG_OPENNING);
        _soundPlayer.PlayLooping();
    }

    public void PlayThriller()
    {
        SetSoundLocation(SONG_THRILLER);
        _soundPlayer.PlayLooping();
    }

    public void PlayQuestionSelection(int delay = 3)
    {
        SetSoundLocation(QUESTION_SELECTION);
        _soundPlayer.Play();

        Thread.Sleep(TimeSpan.FromSeconds(delay));
    }

    public void PlayGameOver(int delay = 5)
    {
        SetSoundLocation(SONG_GAME_OVER);
        _soundPlayer.Play();

        Thread.Sleep(TimeSpan.FromSeconds(delay));
    }
}