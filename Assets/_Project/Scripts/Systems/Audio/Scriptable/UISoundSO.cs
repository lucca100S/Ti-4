using UnityEngine;

[CreateAssetMenu(fileName = "UISound", menuName = "Scriptable Objects/Audio/UISound")]
public class UISoundSO : AudioSO
{
    [Header("UI Sound Specific")]
    [Tooltip("Tipo semântico deste som (apenas para organização e buscas).")]
    public UISoundType UIType = UISoundType.Button;

    [Tooltip("Se verdadeiro, este som é preferencialmente 2D e não dever ser spatializado.")]
    public bool Prefer2D = true;

    [Tooltip("Recomendação de prioridade para UI (menor número = mais prioridade).")]
    public int UIPriority = 128;
}
