using System;
using UnityEngine;

/// <summary>
/// Define um Objeto interativoo não voluntário
/// </summary>
[RequireComponent(typeof(Collider))]
public class NonVoluntaryInteractable : InteractableObjects
{
    // Sempre interagível ao colidir, então retornamos true
    public override Func<bool> IsInteractable => () => true;

    public override void Interaction()
    {
        Debug.LogWarning("Interação automática de NonVoluntaryInteractable disparada!");
    }

    // Dispara interação automaticamente quando o objeto com a tag correta entra no trigger
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Interaction(); // chama a interação automaticamente
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.forward);
    }
}

