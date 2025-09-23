using UnityEngine;

/// <summary>
/// Representa um coletável no cenário.
/// Notifica observadores quando é coletado.
/// </summary>
public class Collectables : OptionalInteractableObjects
{
    public override void Interaction()
    {
#if UNITY_EDITOR
        base.Interaction();
        PreviouslyInteracted = true;
#endif
        //Rotina de interação : SFX,VFX,etc
        //Notificar listeners
        CollectableObservable.Instance.NotifyListeners(this);
    }

    public override void InitializeInteractableState()
    {
        base.InitializeInteractableState();
        switch (PreviouslyInteracted)
        {
            case true:
                this.gameObject.SetActive(false);
                break;
            case false:
                Debug.Log($"Interativo coletável {this.gameObject.name} disponível");
                break;
        }
    }
}
