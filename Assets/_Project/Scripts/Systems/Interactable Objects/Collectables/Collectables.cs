using UnityEngine;

/// <summary>
/// Representa um colet�vel no cen�rio.
/// Notifica observadores quando � coletado.
/// </summary>
public class Collectables : OptionalInteractableObjects
{
    public override void Interaction()
    {
        if (IsInteractable())
            CollectableObservable.Instance.NotifyListeners(this);
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(SphereOrigin, Radius);
    }
}
