using UnityEngine;

[CreateAssetMenu(fileName = "Ambience", menuName = "Scriptable Objects/Audio/Ambience")]
public class AmbienceSO : AudioSO
{
    [Header("Ambience Specific")]
    [Tooltip("Se verdadeiro, este som deve persistir entre cenas por padrão.")]
    public bool PersistAcrossScenes = true;

    [Tooltip("Delay aleatório inicial (segundos) para variar início em múltiplas instâncias.")]
    public Vector2 RandomStartOffsetRange = Vector2.zero;

    [Tooltip("Tempo de fade-in recomendado ao iniciar este áudio (segundos).")]
    public float RecommendedFadeIn = 0.5f;

    [Tooltip("Tempo de fade-out recomendado ao parar este áudio (segundos).")]
    public float RecommendedFadeOut = 0.5f;
}
