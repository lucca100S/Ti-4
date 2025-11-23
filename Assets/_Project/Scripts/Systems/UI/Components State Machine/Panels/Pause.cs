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
                break;
            default:
                Debug.Log("Paused from unknown panel");
                break;
        }
        this.gameObject.SetActive(true);
    }
    public override void OnExit(Panel next)
    {
        Debug.Log("Main Menu specific OnExit logic");
        switch (next)
        {
            case HUD:
                var FadeTransition = new FadeTransition(0.5f);
                FadeTransition.PlayExit(this, () =>
                {
                    Debug.Log("Exited to HUD");
                });
                break;
        }
        this.gameObject.SetActive(false);
    }
}
