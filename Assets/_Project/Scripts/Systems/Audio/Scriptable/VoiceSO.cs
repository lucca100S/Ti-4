using UnityEngine;

[CreateAssetMenu(fileName = "Voice", menuName = "Scriptable Objects/Audio/Voice")]
public class VoiceSO : AudioSO
{
    [Header("Voice Specific")]
    [Tooltip("Chave de localização / ID do texto associado a este áudio (opcional).")]
    public string LocalizationKey;

    [Tooltip("Se múltiplas variações de vocal existem (mesmo texto, vozes diferentes), coloque aqui.")]
    public AudioClip[] VariantsByActor;

    [Tooltip("Prioridade de diálogo — diálogos críticos podem duckar música.")]
    [Range(0, 256)]
    public int DialoguePriority = 128;

    [Tooltip("Tempo (s) de offset para sincronização de lipsync (se necessário).")]
    public float LipSyncOffset = 0f;

    public override AudioClip GetRandomClip()
    {
        // prefer VariantsByActor se presente, senão usa Clips herdados
        if (VariantsByActor != null && VariantsByActor.Length > 0)
            return VariantsByActor[UnityEngine.Random.Range(0, VariantsByActor.Length)];
        return base.GetRandomClip();
    }
}
