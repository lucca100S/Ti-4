using System;
using UnityEngine;

/// <summary>
/// Classe abstrata que usa o contrato IInteractable e define um padrão na condição de interação 
/// </summary>
public abstract class InteractableObjects : MonoBehaviour, IInteractable
{
    [Header("Detection")]
    public float radius = 3f;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected string targetTag;

    private Collider[] _results = new Collider[10]; // Pre-allocated array para performance

    // Func que retorna true se houver target dentro do raio
    public virtual Func<bool> IsInteractable => IsTagInsideSphere;

    public bool IsTagInsideSphere()
    {
        int count = UnityEngine.Physics.OverlapSphereNonAlloc(transform.position, radius, _results, layerMask);

        for (int i = 0; i < count; i++)
        {
            if (_results[i] != null && _results[i].CompareTag(targetTag))
                return true;
        }

        return false;
    }

    public abstract void Interaction();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}