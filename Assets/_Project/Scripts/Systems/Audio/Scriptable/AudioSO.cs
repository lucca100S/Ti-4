using System;
using UnityEngine;

/// <summary>
/// Defines the base abstraction for audio objects, used by concrete classes to define project audio assets.
/// </summary>
public abstract class AudioSO : ScriptableObject
{
    [Header("Identification")]
    [Tooltip("ID do audio (preenchido pelo CodeGen ou manualmente).")]
    public AudioId Id;

    [Header("Clips / Variants")]
    [Tooltip("Lista de variantes. O playback deve escolher uma variante aleatória quando aplicável.")]
    public AudioClip[] Clips;

    [Header("Behavior (design intent)")]
    [Tooltip("Se verdadeiro, o som deve repetir por padrão (intenção de design). Playback pode sobrescrever.")]
    public bool LoopByDefault = false;

    [Range(0f, 1f)]
    [Tooltip("Nível base do asset para mixagem. Multiplicador aplicado antes dos volumes globais.")]
    public float DefaultGain = 1f;

    [TextArea(2, 6)]
    [Tooltip("Descrição curta do propósito/uso deste som (ajuda designers/artistas).")]
    public string Description;

    /// <summary>
    /// Runtime flag used by the playback system.
    /// </summary>
    public bool Loop { get; internal set; }

    /// <summary>
    /// Retorna uma variante aleatória, ou null se não houver clips.
    /// Implementação básica compartilhada — derivações podem expor helpers adicionais.
    /// </summary>
    public virtual AudioClip GetRandomClip()
    {
        if (Clips == null || Clips.Length == 0) return null;
        return Clips[UnityEngine.Random.Range(0, Clips.Length)];
    }
}
