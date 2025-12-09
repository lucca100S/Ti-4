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
    [Range(0f, 1f)] public static float MasterVolume = 1f;
    [Range(0f, 1f)] public static float MusicVolume = 1f;
    [Range(0f, 1f)] public static float SFXVolume = 1f;

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

    private void UpdateActiveVolumes()
    {
        foreach (var kvp in active)
        {
            AudioId id = kvp.Key;
            List<AudioSource> sources = kvp.Value;

            // Determina se é música ou SFX
            bool isMusic = id.ToString().Contains("Music");
            float typeMultiplier = isMusic ? MusicVolume : SFXVolume;

            // Pega o AudioSO correspondente no registry
            AudioSO audioSO = AudioRegistry.Instance.Get(id); // Assumindo que RegistryBase tem método Get()
            float defaultGain = audioSO != null ? audioSO.DefaultGain : 1f;

            foreach (var src in sources)
            {
                if (src == null) continue;
                // Agora aplica MasterVolume, tipo e DefaultGain
                src.volume = MasterVolume * typeMultiplier * defaultGain;
            }
        }
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

    void Start()
    {
        EventBus.Publish(new ChangeMasterVolumeEvent(MasterVolume));
        EventBus.Publish(new ChangeSFXVolumeEvent(SFXVolume));
        EventBus.Publish(new ChangeMusicVolumeEvent(MusicVolume));
        EventBus.Subscribe<ChangeMasterVolumeEvent>(evt =>
        {
            MasterVolume = evt.MasterVolume;
            UpdateActiveVolumes();
        });
        EventBus.Subscribe<ChangeMusicVolumeEvent>(evt =>
        {
            MusicVolume = evt.MusicVolume;
            UpdateActiveVolumes();
        });
        EventBus.Subscribe<ChangeSFXVolumeEvent>(evt =>
        {
            SFXVolume = evt.SFXVolume;
            UpdateActiveVolumes();
        });
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
            Debug.LogWarning($"[AudioManager] AudioSO '{audio.name}' has Id == None.");
        }

        // Obtém lista de instâncias ativas ou cria uma nova
        if (!active.TryGetValue(audio.Id, out var list))
        {
            list = new List<AudioSource>();
            active[audio.Id] = list;
        }

        // Limite de instâncias para MusicSO
        if (audio is MusicSO music && list.Count >= music.MaxSimultaneousInstances)
        {
            // Retorna a instância mais antiga para não criar som duplicado
            return list[0];
        }

        // Controle de PlaybackForce
        bool anyPlaying = list.Exists(s => s != null && s.isPlaying);
        if (force == PlaybackForce.IgnoreIfPlaying && anyPlaying)
            return list.Find(s => s != null && s.isPlaying);

        if (force == PlaybackForce.ForceRestart)
        {
            foreach (var s in list)
                if (s != null) StopAndCleanupSource(s);
            list.Clear();
        }

        // Escolhe o clip a tocar
        AudioClip clip = (audio is MusicSO musicSO) ? musicSO.GetLoopClip() : audio.GetRandomClip();
        if (clip == null)
        {
            Debug.LogWarning($"[AudioManager] AudioSO '{audio.name}' has no clips.");
            return null;
        }

        // Cria o GameObject e AudioSource
        GameObject go = new GameObject($"Audio_{audio.name}");
        if (spatial && worldPosition.HasValue)
            go.transform.position = worldPosition.Value;
        else
            go.transform.SetParent(this.transform, false);

        AudioSource src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.loop = audio.LoopByDefault;

        // Ajusta pitch se SFX
        if (audio is SoundEffectSO sfx)
        {
            float randomPitch = UnityEngine.Random.Range(sfx.PitchRange.x, sfx.PitchRange.y);
            src.pitch = randomPitch;
        }
        else
        {
            src.pitch = 1f;
        }

        // Volume ajustado
        float typeMultiplier = (audio is MusicSO) ? MusicVolume : SFXVolume;
        src.volume = audio.DefaultGain * MasterVolume * typeMultiplier;
        src.spatialBlend = spatial ? 1f : 0f;

        src.Play();
        list.Add(src);

        // AutoDestroy se necessário
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

    //UI Event
    public void StopAll()
    {
        foreach (var kvp in active)
        {
            List<AudioSource> sources = kvp.Value;
            foreach (var src in sources)
            {
                if (src != null)
                {
                    StopAndCleanupSource(src);
                }
            }
        }

        // Limpa o dicionário
        active.Clear();
    }

}
