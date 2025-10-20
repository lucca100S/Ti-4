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
    List<string> collectableNames = new List<string>();
    private void Start()
    {
        TextMeshPro.text = string.Empty;
        CollectableObservable.Instance?.SubscribeListeners(this);
    }

    public override void OnCollectable(Collectables item)
    {
        collectableNames.Add(item.name);
        TextMeshPro.text += $"{item.name}\n";
    }

    public override void SetCollectedCollectables(List<Collectables> itens)
    {
        throw new System.NotImplementedException();
    }
}
