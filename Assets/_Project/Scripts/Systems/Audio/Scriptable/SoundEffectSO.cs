using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect", menuName = "Scriptable Objects/Audio/SFX")]
public class SoundEffectSO : AudioSO
{
    [Header("SFX Specific")]
    [Tooltip("Pitch aleatório (min, max) aplicado no playback para variar repetição.")]
    public Vector2 PitchRange = new Vector2(1f, 1f);

    [Tooltip("Número máximo recomendado de instâncias simultâneas deste SFX.")]
    public int MaxSimultaneousInstances = 4;

    [Tooltip("Cooldown mínimo entre execuções deste SFX (segundos).")]
    public float Cooldown = 0f;

    [Tooltip("Prioridade de reprodução (0 = mais alta prioridade).")]
    [Range(0, 256)]
    public int Priority = 128;

    /// <summary>
    /// Helper que retorna um clip aleatório e um pitch sugerido dentro do range.
    /// O PlaybackSystem pode usar isso para aplicar pitch localmente.
    /// </summary>
    public (AudioClip clip, float pitch) SampleVariantWithPitch()
    {
        var clip = GetRandomClip();
        float pitch = UnityEngine.Random.Range(PitchRange.x, PitchRange.y);
        return (clip, pitch);
    }
}
