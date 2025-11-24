public class SFXVolumeSlider : VolumeSlider<ChangeSFXVolumeEvent>
{
    public void OnEnable()
    {
        this.Set(AudioManager.Instance.SFXVolume);        
    }
    public override void OnVolumeChanged()
    {
        EventBus.Publish(new ChangeSFXVolumeEvent(this.Get()));
    }
}
