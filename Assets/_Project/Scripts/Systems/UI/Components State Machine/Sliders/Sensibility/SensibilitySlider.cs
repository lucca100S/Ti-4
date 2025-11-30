using UnityEngine;

public class SensibilitySlider<T> : SliderBase<SensibilitySliderLimits> where T : struct
{
    public virtual void OnSensibiltyChanged()
    {
        Debug.Log(base.ToString() + "is being called on base class instead on concrete class");
    }
}
