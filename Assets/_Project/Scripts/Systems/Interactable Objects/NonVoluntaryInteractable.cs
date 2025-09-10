using System;
using UnityEngine;

/// <summary>
/// Define um Objeto interativoo n�o volunt�rio
/// </summary>
[RequireComponent(typeof(Collider))]
public class NonVoluntaryInteractable : InteractableObjects
{
    // Sempre interag�vel ao colidir, ent�o retornamos true
    public override Func<bool> IsInteractable => () => true;

    public override void Interaction()
    {
        Debug.LogWarning("Intera��o autom�tica de NonVoluntaryInteractable disparada!");
    }

    // Dispara intera��o automaticamente quando o objeto com a tag correta entra no trigger
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Interaction(); // chama a intera��o automaticamente
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.forward);
    }
}

