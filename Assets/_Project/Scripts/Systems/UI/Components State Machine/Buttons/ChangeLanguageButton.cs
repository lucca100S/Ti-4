using UnityEngine;

public class ChangeLanguageButton : Button
{
    public GameLanguages targetLanguage;
    public override void OnClik()
    {
        EventBus.Publish(new GameLanguageChangeEvent(targetLanguage));
    }
}
