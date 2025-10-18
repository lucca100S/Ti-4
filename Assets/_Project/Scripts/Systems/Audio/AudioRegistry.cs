using System.Collections.Generic;


/// <summary>
/// Concrete registry for AudioSO objects keyed by AudioId.
/// </summary>
public class AudioRegistry : RegistryBase<AudioSO, AudioId>
{
    public List<SceneAudioSO> _scenes;
}
