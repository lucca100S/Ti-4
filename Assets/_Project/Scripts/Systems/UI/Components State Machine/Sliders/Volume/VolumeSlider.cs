using UnityEngine;

public class VolumeSlider<T> : SliderBase<VolumeSliderLimits> where T : struct
{
    public virtual void OnVolumeChanged()
    {
        Debug.Log(base.ToString() + "is being called on base class instead on concrete class");
    }
}
