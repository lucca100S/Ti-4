using UnityEngine;

public class AudioSettings : Panel
{
    public override void OnEnter(Panel previous)
    {
        this.gameObject.SetActive(true);
    }

    public override void OnExit(Panel next)
    {
        this.gameObject.SetActive(false);
    }
}
