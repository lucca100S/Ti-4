using System;
using UnityEngine;
/// <summary>
/// Define um Objeto interativoo voluntário
/// </summary>
public class VoluntaryInteractable : InteractableObjects
{
    public override Func<bool> IsInteractable => IsTagInsideSphere;

    protected Collider[] _results = new Collider[10]; // Pre-allocated array para performance

    public override void Interaction()
    {
        throw new System.NotImplementedException();
    }

    // Func que retorna true se houver target dentro do raio
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
