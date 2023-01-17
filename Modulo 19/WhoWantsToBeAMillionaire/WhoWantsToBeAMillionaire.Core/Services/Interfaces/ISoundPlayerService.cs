namespace WhoWantsToBeAMillionaire.Core.Services.Interfaces;

public interface ISoundPlayerService
{
    string SoundLocation { get; set; }
    void Play();
    void PlayLooping();
}