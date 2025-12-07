using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class SensibilitySlider : SliderBase<SensibilitySliderLimits>
{
    public PlayerController playerController;
    public void OnEnable()
    {
        this.GetComponent<Slider>().value = FindAnyObjectByType<SaveHandler>().gameSettingsData.sensitivity;
    }
    public virtual void OnSensibiltyChanged()
    {
        playerController.ChangeCameraSensitivity(this.GetComponent<Slider>().value);
        EventBus.Publish(new OnSensibilityChange { sensibility = this.GetComponent<Slider>().value });
    }
}
