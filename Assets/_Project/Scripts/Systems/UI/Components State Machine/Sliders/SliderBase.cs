using Unity.Collections;
using UnityEngine;

public abstract class SliderBase<TLimits> : UIComponent where TLimits : struct
{
    [SerializeField]
    public float Min => (float)typeof(TLimits).GetField("Min").GetValue(null);
    public float Max => (float)typeof(TLimits).GetField("Max").GetValue(null);
    public void Set(float v)
    {
        if (v < Min) v = Min;
        if (v > Max) v = Max;
        this.GetComponent<UnityEngine.UI.Slider>().value = v;
    }

    public float Get() => this.GetComponent<UnityEngine.UI.Slider>().value;
}
