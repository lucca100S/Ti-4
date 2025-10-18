using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the class that holds the audios used in specific scenes
/// </summary>
[CreateAssetMenu(fileName = "SceneAudio", menuName = "Scriptable Objects/SceneAudio")]
public class SceneAudioSO : ScriptableObject
{
    public List<AudioSO> sceneAudios = new List<AudioSO>();
}
