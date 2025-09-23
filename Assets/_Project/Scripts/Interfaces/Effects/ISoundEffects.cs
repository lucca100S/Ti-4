using UnityEngine;
/// <summary>
/// Interface dedicada a centralizar a manipulação individual de SFX
/// </summary>
public interface ISoundEffects
{
    bool SFXIsPlaying { get; set; }
    bool SFXIsPaused { get; set; }
    void SFXPlay();
    void SFXPause();
    void SFXStop();
}