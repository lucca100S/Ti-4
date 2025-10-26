using UnityEngine;

/// <summary>
/// Representa um colet�vel no cen�rio.
/// Notifica observadores quando � coletado.
/// </summary>
public class Collectables : OptionalInteractableObjects
{
    public override void Interaction()
    {
        Debug.Log($"[Collectable] Coletado: {this.gameObject.name}");
        CollectableObservable.Instance?.NotifyListeners(this);
        this.gameObject.SetActive(false);
    }
}
