using UnityEngine;

public class SliderMasterVolume : UIComponentBase
{
    [Tooltip("Novo valor no slider")]
    [Range(0f, 1f)]
    public float newVolume;

    public override void OnTrigger()
    {
        base.OnTrigger();
        newVolume = this.GetComponent<UnityEngine.UI.Slider>().value;
        EventBus.Publish(new MasterVolumeChangeEvent { NewVolume = newVolume });
    }
}



