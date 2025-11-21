using UnityEngine;

public class UITextMeshProElement : UIComponent
{
    public TextElementsContent textElementsContent;
    public TMPro.TMP_Text textMeshProComponent;
    void Awake() => EventBus.Subscribe<GameLanguageChangeEvent>(UpdateTextLanguage);
    void UpdateTextLanguage(GameLanguageChangeEvent eventData)
    {
        if (textMeshProComponent != null)
        {
            textMeshProComponent.text = TextElementsContent.GetTextByLanguage(textElementsContent, eventData.CurrentLanguage);
        }
        else
        {
            GetComponentInChildren<TMPro.TMP_Text>().text = TextElementsContent.GetTextByLanguage(textElementsContent, eventData.CurrentLanguage);
        }
    } 
}
