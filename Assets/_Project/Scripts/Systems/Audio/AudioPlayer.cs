using UnityEngine;

/// <summary>
/// Static facade that gameplay code should call. Internally delegates to AudioManager and AudioRegistry.
/// </summary>
public static class AudioPlayer
{
    /// <summary>
    /// Play by enum id (global / 2D).
    /// </summary>
    public static AudioSource Play(AudioId id, PlaybackForce force = PlaybackForce.Normal)
    {
        var reg = AudioRegistry.Instance;
        if (reg == null)
        {
            Debug.LogWarning("[AudioPlayer] AudioRegistry.Instance not found in scene.");
            return null;
        }

        var so = reg.Get(id);
        if (so == null)
        {
            Debug.LogWarning($"[AudioPlayer] AudioId '{id}' not found in registry.");
            return null;
        }

        return Play(so, force);
    }

    /// <summary>
    /// Play AudioSO (global / 2D)
    /// </summary>
    public static AudioSource Play(AudioSO audio, PlaybackForce force = PlaybackForce.Normal)
    {
        if (AudioManager.Instance == null)
            Debug.LogWarning("[AudioPlayer] AudioManager instance not found; creating one.");
        return AudioManager.Instance.Play(audio, null, spatial: false, force: force);
    }

    /// <summary>
    /// Play at world position (3D spatial).
    /// </summary>
    public static AudioSource PlaySpatial(AudioId id, Vector3 position, PlaybackForce force = PlaybackForce.Normal)
    {
        var reg = AudioRegistry.Instance;
        if (reg == null) { Debug.LogWarning("[AudioPlayer] AudioRegistry.Instance not found in scene."); return null; }
        var so = reg.Get(id);
        if (so == null) { Debug.LogWarning($"[AudioPlayer] AudioId '{id}' not found in registry."); return null; }
        return PlaySpatial(so, position, force);
    }

    public static AudioSource PlaySpatial(AudioSO audio, Vector3 position, PlaybackForce force = PlaybackForce.Normal)
    {
        if (AudioManager.Instance == null)
            Debug.LogWarning("[AudioPlayer] AudioManager instance not found; creating one.");
        return AudioManager.Instance.Play(audio, position, spatial: true, force: force);
    }

    /// <summary>Stop by id</summary>
    public static void Stop(AudioId id)
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.Stop(id);
    }

    /// <summary>Stop by AudioSO</summary>
    public static void Stop(AudioSO audio)
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.Stop(audio);
    }

    /// <summary>Restart by id</summary>
    public static AudioSource Restart(AudioId id)
    {
        var reg = AudioRegistry.Instance;
        if (reg == null) { Debug.LogWarning("[AudioPlayer] AudioRegistry.Instance not found in scene."); return null; }
        var so = reg.Get(id);
        if (so == null) { Debug.LogWarning($"[AudioPlayer] AudioId '{id}' not found in registry."); return null; }
        return AudioManager.Instance.Restart(so);
    }
}
