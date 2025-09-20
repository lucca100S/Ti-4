using UnityEngine;

/// <summary>
/// Representa um checkpoint interagível no cenário.
/// </summary>
public class CheckPoint : OptionalInteractableObjects
{
    public override void Interaction()
    {
        if (IsInteractable()) { /* Lógica de checkpoint */ }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(SphereOrigin, Radius);
    }
}
