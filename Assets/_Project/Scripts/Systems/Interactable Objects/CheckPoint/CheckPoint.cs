using UnityEngine;

/// <summary>
/// Representa um checkpoint interag�vel no cen�rio.
/// </summary>
public class CheckPoint : OptionalInteractableObjects
{
    public override void Interaction()
    {
        if (IsInteractable()) { /* L�gica de checkpoint */ }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(SphereOrigin, Radius);
    }
}
