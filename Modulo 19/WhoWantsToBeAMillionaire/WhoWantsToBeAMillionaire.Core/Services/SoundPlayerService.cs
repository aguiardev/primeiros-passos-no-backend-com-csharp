using System.Diagnostics.CodeAnalysis;
using System.Media;
using WhoWantsToBeAMillionaire.Core.Services.Interfaces;

namespace WhoWantsToBeAMillionaire.Core.Services;

[ExcludeFromCodeCoverage]
public class SoundPlayerService : ISoundPlayerService
{
    private readonly SoundPlayer _soundPlayer;
    
    public string SoundLocation
    {
        get => _soundPlayer.SoundLocation;
        set => _soundPlayer.SoundLocation = value;
    }

    public SoundPlayerService(SoundPlayer soundPlayer) => _soundPlayer = soundPlayer;

    public void Play() => _soundPlayer.Play();

    public void PlayLooping() => _soundPlayer.PlayLooping();
}