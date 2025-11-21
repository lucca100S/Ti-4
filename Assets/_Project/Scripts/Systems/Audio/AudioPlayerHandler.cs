using UnityEngine;

public class AudioPlayerHandler : MonoBehaviour
{
    private void Start()
    {
        PlayStartMusic();
    }

    public void PlayStartMusic()
    {
        AudioPlayer.Play(AudioId.InGameMusic);
    }

    public void PlayEndMusic()
    {
        AudioPlayer.Stop(AudioId.InGameMusic);
        AudioPlayer.Play(AudioId.MenuMusic);
    }
}
