using UnityEngine;

public class Test : MonoBehaviour 
{
    public void Play()
    {
        AudioManager.Instance.Play(FindFirstObjectByType<AudioRegistry>()._scenes[0].sceneAudios[0]);
    }
}
