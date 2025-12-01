using UnityEngine;
using UnityEngine.TextCore.Text;

public class CollectableCounterHandler : UITextMeshProElement
{
    int collectableCount = 0;
    public void UpdateCollectableText()
    {
        collectableCount++;
        if (textMeshProComponent != null)
        {
            textMeshProComponent.text = $"{collectableCount.ToString("D2")}";
        }
    } 
}
