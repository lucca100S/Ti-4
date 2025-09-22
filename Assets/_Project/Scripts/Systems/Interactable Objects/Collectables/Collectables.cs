using UnityEngine;

/// <summary>
/// Representa um coletável no cenário.
/// Notifica observadores quando é coletado.
/// </summary>
public class Collectables : OptionalInteractableObjects
{
    public override void Interaction()
    {
        # if UNITY_EDITOR
        base.Interaction();
        #endif
        //Rotina de interação : SFX,VFX,etc
        //Notificar listeners
        CollectableObservable.Instance.NotifyListeners(this);
    }
}
