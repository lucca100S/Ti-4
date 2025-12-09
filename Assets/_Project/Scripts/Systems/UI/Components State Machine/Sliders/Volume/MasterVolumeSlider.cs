using UnityEngine;
using UnityEngine.UI;
public class MasterVolumeSlider : VolumeSlider<ChangeMasterVolumeEvent>
{
    public void SetValue(ChangeMasterVolumeEvent changeMasterVolumeEvent)
    {
        this.GetComponent<Slider>().value = changeMasterVolumeEvent.MasterVolume;
    }
    void OnEnable()
    {
        this.GetComponent<Slider>().value = AudioManager.MasterVolume;
    }
    public override void OnVolumeChanged()
    {
        EventBus.Publish(new ChangeMasterVolumeEvent(this.Get()));
    }
}
