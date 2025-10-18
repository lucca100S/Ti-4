using UnityEngine;

[CreateAssetMenu(fileName = "Music", menuName = "Scriptable Objects/Audio/Music")]
public class MusicSO : AudioSO
{
    [Header("Music Specific")]
    [Tooltip("Clip de introdução opcional (toque uma vez antes do loop).")]
    public AudioClip IntroClip;

    [Tooltip("Clip principal utilizado em loop (se diferente do primeiro elemento em Clips).")]
    public AudioClip LoopClip;

    [Tooltip("Tempo padrão de crossfade (segundos) ao trocar para esta música.")]
    public float CrossfadeTime = 1.0f;

    [Tooltip("BPM estimado — útil para sincronizações / transições baseadas em tempo.")]
    public int BPM = 0;

    [Tooltip("Se verdadeiro, recomenda streaming do arquivo em disco (útil para músicas longas).")]
    public bool StreamFromDisk = false;

    // Conveniência: se LoopClip estiver vazio, GetRandomClip() usa Clips[0].
    public AudioClip GetLoopClip()
    {
        if (LoopClip != null) return LoopClip;
        return base.GetRandomClip();
    }
}
