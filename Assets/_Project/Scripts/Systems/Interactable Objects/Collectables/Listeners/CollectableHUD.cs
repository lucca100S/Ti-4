using UnityEngine;
using TMPro;
using System.Collections.Generic;
/// <summary>
/// Classe que generaliza listeners do obeserver de coletáveis, para o trabalho de PDJ4, futuramente se adequará as telas do jogo nas quais os coletáveis apareçam
/// </summary>
public class CollectableHUD : MonoBehaviour, ICollectableListener
{
    [SerializeField] private TextMeshProUGUI collectableText;

    void Awake()
    {
        if (CollectableObserver.Instance != null)
            CollectableObserver.Instance.RegisterListener(this);
    }

    void OnDestroy()
    {
        if (CollectableObserver.Instance != null)
            CollectableObserver.Instance.UnregisterListener(this);
    }

    public void OnCollectableUpdated(IReadOnlyList<Collectable> collectables)
    {
        if (collectableText != null)
            collectableText.text = $"Coletáveis: {collectables.Count}";
    }
}
