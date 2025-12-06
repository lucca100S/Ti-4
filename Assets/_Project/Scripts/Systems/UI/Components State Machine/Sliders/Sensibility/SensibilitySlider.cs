using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class SensibilitySlider : SliderBase<SensibilitySliderLimits>
{
    public PlayerController playerController;
    public void OnEnable()
    {
        this.GetComponent<Slider>().value = playerController.CameraInputs.Controllers.First().Input.Gain;
    }
    public virtual void OnSensibiltyChanged()
    {
        playerController.ChangeCameraSensitivity(this.GetComponent<Slider>().value);
    }
}
