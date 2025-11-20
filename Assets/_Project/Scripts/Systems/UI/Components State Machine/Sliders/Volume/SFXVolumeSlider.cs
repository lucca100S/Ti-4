public class SFXVolumeSlider : VolumeSlider<ChangeSFXVolumeEvent>
{
    public override void OnVolumeChanged()
    {
        this.Set(this.GetComponent<UnityEngine.UI.Slider>().value);
        EventBus.Publish(new ChangeSFXVolumeEvent(this.Get()));
    }
}
