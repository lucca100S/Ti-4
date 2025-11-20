using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class Pause : Panel
{
    public GameObject DistortionEffectGlobalVolume;
    public override void OnEnter(Panel previous)
    {
        Debug.Log("Main Menu specific OnEnter logic");
        switch (previous)
        {
            case HUD:
                Debug.Log("Paused from HUD");
                DistortionEffectGlobalVolume.SetActive(true);
                this.gameObject.SetActive(true);
                break;
            default:
                Debug.Log("Paused from unknown panel");
                break;
        }
    }
    public override void OnExit(Panel next)
    {
        Debug.Log("Main Menu specific OnExit logic");
        switch (next)
        {
            case HUD:
                Debug.Log("Exited to Main Menu");
                break;
        }
        if (next is HUD)
        {
            Debug.Log("Resumed to HUD");
            DistortionEffectGlobalVolume.SetActive(false);
        }

        this.gameObject.SetActive(false);
    }
}
