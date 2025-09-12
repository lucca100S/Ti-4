using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Implementação de teste para observer de coletáveis.
/// Incrementa contador na UI ao coletar.
/// </summary>
public class ObserverTest : CollectableObserver
{
    public TMP_Text TextMeshPro;
    int _quantity;

    private void Start()
    {
        _quantity = 0;
        TextMeshPro.text = _quantity.ToString();
        CollectableObservable.Instance?.SubscribeListeners(this);
    }

    public override void OnCollectable(Collectables item)
    {
        Debug.Log("OnCollectable");
        _quantity++;
        TextMeshPro.text = _quantity.ToString();
    }

    public override void SetCollectedCollectables(List<Collectables> itens)
    {
        throw new System.NotImplementedException();
    }
}
