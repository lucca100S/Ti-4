using UnityEngine;
using UnityEngine.UI;
public class MasterVolumeSlider : VolumeSlider<ChangeMasterVolumeEvent>
{
    public void OnEnable()
    {
        this.Set(AudioManager.Instance.MasterVolume);        
    }
    public override void OnVolumeChanged()
    {
        EventBus.Publish(new ChangeMasterVolumeEvent(this.Get()));
    }
}
