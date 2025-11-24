using UnityEngine;

public class TriggerMusic : MonoBehaviour
{
    public AudioId audioId;

    void Start()
    {
        AudioPlayer.Play(audioId);
    }
}
