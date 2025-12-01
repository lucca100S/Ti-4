using System.Collections.Generic;
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
//SUBPANEL ACTIVATION EVENT
[System.Serializable]
public struct ActivateSubPanelEvent
{
    public Panel subPanel;
    public Panel[] otherSubPanels;
    public ActivateSubPanelEvent(Panel subPanel, Panel[] otherSubPanels)
    {
        this.subPanel = subPanel;
        this.otherSubPanels = otherSubPanels;
    }
}
[System.Serializable]
public struct ToggleSubPanelEvent
{
    public Panel subPanel;
    public bool activate;
    public ToggleSubPanelEvent(Panel subPanel, bool activate)
    {
        this.subPanel = subPanel;
        this.activate = activate;
    }
}

//COLLECTABLE COUNT EVENT
public struct AddCollectableCountEvent{}

//APPLICATION QUIT EVENT
public struct ApplicationQuitEvent {}
[System.Serializable]
public struct OpenSectionEvent
{
    public GameObject section;
    public List<GameObject> sectionsToClose;
    public OpenSectionEvent(GameObject section, List<GameObject> sectionsToClose)
    {
        this.section = section;
        this.sectionsToClose = sectionsToClose;
    }
}
[System.Serializable]
public struct CloseSectionEvent
{
    public List<GameObject> section;
    public CloseSectionEvent(List<GameObject> section)
    {
        this.section = section;
    }
}

public struct OnSensibilityChange
{
    float sensibilityX;
    float sensibilityY;

    public OnSensibilityChange(float sensibilityX, float sensibilityY)
    {
        this.sensibilityX = sensibilityX;
        this.sensibilityY = sensibilityY;
    }
}

public struct SaveData{}
public struct LoadData{}