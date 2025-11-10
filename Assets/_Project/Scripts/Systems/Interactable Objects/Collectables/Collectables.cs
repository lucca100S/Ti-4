using System;
using UnityEngine;

/// <summary>
/// Representa um coletável no cenário.
/// Notifica observadores quando é coletado.
/// </summary>
public class Collectables : OptionalInteractableObjects
{
    public CollectableData collectableData;
    public override void Interaction()
    {
        if(!collectableData.Collected)
        {
            Debug.Log($"[Collectable] Coletado: {this.gameObject.name}");
            CollectableObservable.Instance?.NotifyListeners(this);
            this.gameObject.SetActive(false);
            collectableData.Collected = true;
            FindAnyObjectByType<StageDataHandler>().UpdateData(this);
        }
    }
}

[Serializable]
public struct CollectableData
{
    public string ID;
    public bool Collected;
}