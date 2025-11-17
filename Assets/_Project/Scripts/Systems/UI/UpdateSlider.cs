using UnityEngine;
using UnityEngine.UI;

public class UpdateSlider : MonoBehaviour
{
    public VolumeType volumeType;
    public void OnEnable()
    {
        Slider slider = this.GetComponent<UnityEngine.UI.Slider>();
        switch (volumeType)
        {
            case VolumeType.Master:
                slider.value = AudioManager.Instance.MasterVolume;
                break;
            case VolumeType.Music:
                slider.value = AudioManager.Instance.MusicVolume;
                break;
            case VolumeType.SFX:
                slider.value = AudioManager.Instance.SFXVolume;
                break;
            default:
                Debug.Log("Erro na identificação por VolumeType");
                break;
        }
    }
}

public enum VolumeType { Master, Music, SFX }
