using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimatedTextMeshProElement : UIComponent
{
    [SerializeField]
    private List<TextElementsContent> AnimatedSequenceTextElement;
    [SerializeField]
    private float ChangeToNextInSequenceInterval = 0.1f;
    public void OnEnable()
    {
        StartCoroutine(AnimatedTextSequenceCoroutine());
    }

    private System.Collections.IEnumerator AnimatedTextSequenceCoroutine()
    {
        int index = 0;
        var textComponent = this.GetComponentInChildren<TMPro.TMP_Text>();
        while (true)
        {
            if (AnimatedSequenceTextElement.Count == 0)
            {
                yield return null;
            }
            else
            {
                var currentLanguage = UIManager.CurrentLanguage;
                var textToShow = TextElementsContent.GetTextByLanguage(AnimatedSequenceTextElement[index], currentLanguage);
                textComponent.text = textToShow;

                index = (index + 1) % AnimatedSequenceTextElement.Count;
                yield return new WaitForSeconds(ChangeToNextInSequenceInterval);
            }
        }
    }
}
