using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe base para qualquer observer de coletáveis.
/// Cuida de se inscrever/desinscrever automaticamente.
/// </summary>
public abstract class CollectableObserver : MonoBehaviour, IObserver<Collectables>
{
    protected void OnDisable()
    {
        CollectableObservable.Instance?.UnsubscribeListeners(this);
    }

    public abstract void OnCollectable(Collectables item);
    public abstract void SetCollectedCollectables(List<Collectables> itens);
}
