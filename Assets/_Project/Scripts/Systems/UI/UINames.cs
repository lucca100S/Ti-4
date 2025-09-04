using System;

/// <summary>
/// Elucida os painéis presentes na interface do jogo
/// </summary>
public enum UINames
{
    PucLogo,
    GameLogo,
    MainMenu,
    GeneralSettings,
    AudioSettings,
    KeyMapSettings,
    Collectables,
    Defeat,
    Features,
    Help,
    Loading,
    MainMenuCredits,
    EndingCredits,
    Pause,
    Saving
}

/// <summary>
/// Classe que extende o Enumerator UINames para através dela ser possível a relação entre o nome do Panel em cena e o nome em script
/// </summary>
public static class UINamesExtensions
{
    public static string GetPanelName(this UINames name)
    {
        //Panel name pattern: "Name-Panel"
        return $"{name}-Panel";
    }
}