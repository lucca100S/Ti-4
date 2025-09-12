using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Segundo teste de observer, desta vez decrementando contador.
/// </summary>
public class ObserverSecondTest : CollectableObserver
{
    [SerializeField]
    public TMP_Text TextMeshPro;
    int _quantity;

    private void Start()
    {
        _quantity = 10;
        TextMeshPro.text = _quantity.ToString();
        CollectableObservable.Instance?.SubscribeListeners(this);
    }

    public override void OnCollectable(Collectables item)
    {
        _quantity -= 1;
        TextMeshPro.text = _quantity.ToString();
    }

    public override void SetCollectedCollectables(List<Collectables> itens)
    {
        throw new System.NotImplementedException();
    }
}
