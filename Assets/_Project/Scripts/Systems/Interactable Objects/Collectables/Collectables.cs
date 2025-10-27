using UnityEngine;

/// <summary>
/// Representa um coletável no cenário.
/// Notifica observadores quando é coletado.
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
