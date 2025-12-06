using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Collections.Generic;
public class CollectableCounterHandler : UITextMeshProElement, ILoadable
{
    int collectableCount = 0;
    private List<Collectables> collectables = new();
    public void UpdateCollectableText()
    {
        collectableCount++;
        if (textMeshProComponent != null)
        {
            textMeshProComponent.text = $"{collectableCount.ToString("D2")}";
        }
    }

    public void LoadData()
    {
        collectables.AddRange(
    FindObjectsByType<Collectables>(FindObjectsInactive.Include, FindObjectsSortMode.None)
);
        foreach (var collectable in collectables)
        {
            if (collectable.collectableSaveData.isCollected)
            {
                collectableCount++;
            }
        }
    }

    void OnEnable()
    {
        LoadData();
    }
}
