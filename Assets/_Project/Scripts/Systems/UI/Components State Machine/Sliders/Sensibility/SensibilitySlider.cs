using System.Linq;
using Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SensibilitySlider : SliderBase<SensibilitySliderLimits>
{
    public PlayerController playerController;
    public void OnEnable()
    {
        this.GetComponent<Slider>().value = FindAnyObjectByType< CinemachineInputAxisController>().Controllers.First().Input.Gain;
    }
    public virtual void OnSensibiltyChanged()
    {
        playerController.ChangeCameraSensitivity(this.GetComponent<Slider>().value);
        EventBus.Publish(new OnSensibilityChange { sensibility = this.GetComponent<Slider>().value });
    }
}
