using UnityEngine;
//LANGUAGE SETTINGS EVENTS
public struct GameLanguageChangeEvent
{
    GameLanguages currentLanguage;

    public GameLanguageChangeEvent(GameLanguages currentLanguage)
    {
        this.currentLanguage = currentLanguage;
    }

    public GameLanguages CurrentLanguage => currentLanguage;
}
//AUDIO SETTINGS EVENTS
public struct ChangeMasterVolumeEvent
{
    float masterVolume;

    public ChangeMasterVolumeEvent(float masterVolume)
    {
        this.masterVolume = masterVolume;
    }

    public float MasterVolume => masterVolume;
}

public struct ChangeMusicVolumeEvent
{
    float musicVolume;

    public ChangeMusicVolumeEvent(float musicVolume)
    {
        this.musicVolume = musicVolume;
    }

    public float MusicVolume => musicVolume;
}

public struct ChangeSFXVolumeEvent
{
    float sfxVolume;

    public ChangeSFXVolumeEvent(float sfxVolume)
    {
        this.sfxVolume = sfxVolume;
    }

    public float SFXVolume => sfxVolume;
}

//COLLECTABLES SETTINGS EVENTS
public struct CollectablesSliderEvent
{
    int collectablesAmount;

    public CollectablesSliderEvent(int collectablesAmount)
    {
        this.collectablesAmount = collectablesAmount;
    }

    public int CollectablesAmount => collectablesAmount;
}

//PANEL CHANGE EVENT
[System.Serializable]
public struct ChangePanelEvent
{
    public Panel currentPanel;
    public Panel targetPanel;
    public ChangePanelEvent(Panel currentPanel,Panel targetPanel)
    {
        this.targetPanel = targetPanel;
        this.currentPanel = currentPanel;
    }
}