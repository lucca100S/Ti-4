using UnityEngine.UI;
public class MusicVolumeSlider : VolumeSlider<ChangeMusicVolumeEvent>
{
    public void SetValue(ChangeMusicVolumeEvent changeMusicVolumeEvent)
    {
        this.GetComponent<Slider>().value = changeMusicVolumeEvent.MusicVolume;
    }
    void OnEnable()
    {
        this.GetComponent<Slider>().value = AudioManager.MusicVolume;
    }
    public override void OnVolumeChanged()
    {
        EventBus.Publish(new ChangeMusicVolumeEvent(this.Get()));
    }
}
