using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Collections.Generic;
public class CollectableCounterHandler : UITextMeshProElement
{
    public int collectableCount = 0;
    public void UpdateCollectableText()
    {
        collectableCount++;
        if (textMeshProComponent != null)
        {
            textMeshProComponent.text = $"{collectableCount.ToString("D2")}";
        }
    }
}
