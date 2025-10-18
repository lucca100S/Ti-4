using System.Collections.Generic;
using UnityEngine.SceneManagement;


/// <summary>
/// Concrete registry for AudioSO objects keyed by AudioId.
/// </summary>
public class AudioRegistry : RegistryBase<AudioSO, AudioId>
{
    // ----------------------
    // Registry
    // ----------------------
    public List<SceneAudioSO> _scenesAudio;

    // ----------------------
    // Singleton
    // ----------------------
    private static RegistryBase<AudioSO, AudioId> _instance;
    public static RegistryBase<AudioSO, AudioId> Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var scene in _scenesAudio)
        {
            for(int i = 0; i < scene.sceneAudios.Count; i++) 
            {
                Register(scene.sceneAudios[i].Id, scene.sceneAudios[i]);
            }
        }

#if UNITY_EDITOR
        ShowAllKeys();
#endif
    }

}
