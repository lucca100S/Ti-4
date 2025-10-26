using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null) EnsureExists();
            return _instance;
        }
    }

    [Header("Global Volumes")]
    [Range(0f, 1f)] public float MasterVolume = 1f;
    [Range(0f, 1f)] public float MusicVolume = 1f;
    [Range(0f, 1f)] public float SFXVolume = 1f;

    [Header("Pool / Settings")]
    [Tooltip("If true, non-looping SFX GameObjects will be destroyed after playback ends.")]
    public bool AutoDestroyNonLooping = false;

    // Internal registry of active AudioSources per AudioId (multiple instances allowed)
    private readonly Dictionary<AudioId, List<AudioSource>> active = new Dictionary<AudioId, List<AudioSource>>();

    // Ensure an instance exists in scene (creates GameObject if necessary)
    private static void EnsureExists()
    {
        var existing = FindFirstObjectByType<AudioManager>();
        if (existing != null)
        {
            _instance = existing;
            return;
        }

        var go = new GameObject("AudioManager");
        _instance = go.AddComponent<AudioManager>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Could update real-time DSP effects or global mix automation here.
    }

    #region Public low-level API (used by AudioPlayer)

    public AudioSource Play(AudioSO audio, Vector3? worldPosition = null, bool spatial = false, PlaybackForce force = PlaybackForce.Normal)
    {
        if (audio == null)
        {
            Debug.LogWarning("[AudioManager] Play called with null audio.");
            return null;
        }

        if (audio.Id == AudioId.None)
        {
            Debug.LogWarning($"[AudioManager] AudioSO '{audio.name}' has Id == None. Ensure CodeGen populated AudioId or assign manually.");
        }

        // Manage existing instances
        if (active.TryGetValue(audio.Id, out var list))
        {
            bool anyPlaying = list.Exists(s => s != null && s.isPlaying);
            if (force == PlaybackForce.IgnoreIfPlaying && anyPlaying)
                return list.Find(s => s != null && s.isPlaying); // return a playing source

            if (force == PlaybackForce.ForceRestart)
            {
                // stop & cleanup existing
                foreach (var s in list)
                {
                    if (s != null) StopAndCleanupSource(s);
                }
                list.Clear();
            }
            // else Normal: allow multiple instances
        }
        else
        {
            list = new List<AudioSource>();
            active[audio.Id] = list;
        }

        var clip = audio.GetRandomClip();
        if (clip == null)
        {
            Debug.LogWarning($"[AudioManager] AudioSO '{audio.name}' has no clips.");
            return null;
        }

        GameObject go = new GameObject($"Audio_{audio.name}");
        if (spatial && worldPosition.HasValue)
            go.transform.position = worldPosition.Value;
        else
            go.transform.SetParent(this.transform, false);

        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.loop = audio.LoopByDefault;
        float typeMultiplier = 1f;
        if (audio is MusicSO) typeMultiplier = MusicVolume;
        else typeMultiplier = SFXVolume;

        src.volume = Mathf.Clamp01(audio.DefaultGain * MasterVolume * typeMultiplier);
        src.spatialBlend = spatial ? 1f : 0f;

        src.Play();
        list.Add(src);

        if (AutoDestroyNonLooping && !audio.LoopByDefault)
        {
            StartCoroutine(DoAutoCleanup(src, audio.Id, clip.length));
        }

        return src;
    }

    public void Stop(AudioSO audio)
    {
        if (audio == null) return;
        if (!active.TryGetValue(audio.Id, out var list)) return;
        foreach (var s in list)
            if (s != null) StopAndCleanupSource(s);
        list.Clear();
    }

    public void Stop(AudioId id)
    {
        if (!active.TryGetValue(id, out var list)) return;
        foreach (var s in list)
            if (s != null) StopAndCleanupSource(s);
        list.Clear();
    }

    public AudioSource Restart(AudioSO audio, Vector3? position = null, bool spatial = false)
    {
        Stop(audio);
        return Play(audio, position, spatial, PlaybackForce.ForceRestart);
    }

    #endregion

    #region Internal helpers

    private IEnumerator DoAutoCleanup(AudioSource src, AudioId id, float duration)
    {
        if (src == null) yield break;
        yield return new WaitForSeconds(duration + 0.05f);
        if (src.isPlaying) src.Stop();
        RemoveSourceFromActive(id, src);
        if (src.gameObject != null) Destroy(src.gameObject);
    }

    private void RemoveSourceFromActive(AudioId id, AudioSource src)
    {
        if (!active.TryGetValue(id, out var list)) return;
        list.Remove(src);
        if (list.Count == 0) active.Remove(id);
    }

    private void StopAndCleanupSource(AudioSource src)
    {
        if (src == null) return;
        try { src.Stop(); } catch { }
        if (src.gameObject != null) Destroy(src.gameObject);
    }

    #endregion
}
