public class MasterVolumeSlider : VolumeSlider<ChangeMasterVolumeEvent>
{
    public override void OnVolumeChanged()
    {
        this.Set(this.GetComponent<UnityEngine.UI.Slider>().value);
        EventBus.Publish(new ChangeMasterVolumeEvent(this.Get()));
    }
}
