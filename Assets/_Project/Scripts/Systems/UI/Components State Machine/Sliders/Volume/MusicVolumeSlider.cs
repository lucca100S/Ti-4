public class MusicVolumeSlider : VolumeSlider<ChangeMusicVolumeEvent>
{
    public void OnEnable()
    {
        this.Set(AudioManager.Instance.MusicVolume);        
    }
    public override void OnVolumeChanged()
    {
        EventBus.Publish(new ChangeMusicVolumeEvent(this.Get()));
    }
}
