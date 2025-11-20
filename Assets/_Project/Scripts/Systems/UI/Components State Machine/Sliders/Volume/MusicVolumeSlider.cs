public class MusicVolumeSlider : VolumeSlider<ChangeMusicVolumeEvent>
{
    public override void OnVolumeChanged()
    {
        this.Set(this.GetComponent<UnityEngine.UI.Slider>().value);
        EventBus.Publish(new ChangeMusicVolumeEvent(this.Get()));
    }
}
