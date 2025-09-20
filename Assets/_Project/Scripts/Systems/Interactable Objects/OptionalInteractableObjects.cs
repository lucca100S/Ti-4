using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Classe base para objetos interag�veis opcionais.
/// Implementa detec��o por esfera e l�gica de intera��o gen�rica.
/// </summary>
public abstract class OptionalInteractableObjects : MonoBehaviour, IInteractable, ISurroundingsSphereDetection
{
    public Func<bool> IsInteractable => SurroundingsSphereDetection;
    public float Radius { get; set; }
    public LayerMask CollisionMask { get; set; }
    public HashSet<string> CollisionTags { get; set; }
    public Vector3 SphereOrigin { get; set; }

    public virtual void Interaction()
    {
        if (IsInteractable()) { /* L�gica b�sica opcional */ }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(SphereOrigin, Radius);
    }

    public bool SurroundingsSphereDetection()
    {
        var foundColliders = UnityEngine.Physics.OverlapSphere(SphereOrigin, Radius, CollisionMask)
            .Where(c => CollisionTags.Contains(c.tag))
            .ToList();

        if (foundColliders.Count > 0)
        {
            foreach (var col in foundColliders)
                Debug.Log($"Collider v�lido: {col.name}, {col.gameObject.tag}");

            foundColliders.Clear();
            return true;
        }

        foundColliders.Clear();
        return false;
    }
}
