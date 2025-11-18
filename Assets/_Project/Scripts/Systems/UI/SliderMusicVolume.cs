using UnityEngine;

public class SliderMusicVolume : UIComponentBase
{
    [Tooltip("Novo valor no slider")]
    [Range(0f, 1f)]
    public float newVolume;
    public override void OnTrigger()
    {
        base.OnTrigger();
        newVolume = this.GetComponent<UnityEngine.UI.Slider>().value;
        EventBus.Publish(new MusicVolumeChangeEvent { NewVolume = newVolume });
    }
}