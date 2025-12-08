using UnityEngine.UI;
public class SFXVolumeSlider : VolumeSlider<ChangeSFXVolumeEvent>
{
    public void SetValue(ChangeSFXVolumeEvent changeSFXVolumeEvent)
    {
        this.GetComponent<Slider>().value = changeSFXVolumeEvent.SFXVolume;
    }
    public override void OnVolumeChanged()
    {
        EventBus.Publish(new ChangeSFXVolumeEvent(this.Get()));
    }
}
