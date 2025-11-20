using Unity.Collections;
using UnityEngine;

public abstract class SliderBase<TLimits> : UIComponent where TLimits : struct
{
    [SerializeField]
    private float _value;
    public float Min => (float)typeof(TLimits).GetField("Min").GetValue(null);
    public float Max => (float)typeof(TLimits).GetField("Max").GetValue(null);

    void Awake()
    {
       Set(this.GetComponent<UnityEngine.UI.Slider>().value); 
    }

    public void Set(float v)
    {
        if (v < Min) v = Min;
        if (v > Max) v = Max;
        _value = v;
    }

    public float Get() => _value;
}
