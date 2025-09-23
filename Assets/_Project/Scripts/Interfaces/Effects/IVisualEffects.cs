using UnityEngine;
/// <summary>
/// Interface dedicada a centralizar a manipulação individual de VFX
/// </summary>
public interface IVisualEffects
{
    bool VFXIsPlaying { get; set; }
    bool VFXIsPaused { get; set; }
    void VFXPlay();
    void VFXPause();
    void VFXStop();
}
