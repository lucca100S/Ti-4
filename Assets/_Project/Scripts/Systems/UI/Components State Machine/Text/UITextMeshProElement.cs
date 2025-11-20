using UnityEngine;

public class UITextMeshProElement : UIComponent
{
    public TextElementsContent textElementsContent;
    void Awake() => EventBus.Subscribe<GameLanguageChangeEvent>(UpdateTextLanguage);
    void UpdateTextLanguage(GameLanguageChangeEvent eventData) => this.GetComponentInChildren<TMPro.TMP_Text>().text = TextElementsContent.GetTextByLanguage(textElementsContent,eventData.CurrentLanguage);   
}
