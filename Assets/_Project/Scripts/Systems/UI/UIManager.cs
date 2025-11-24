using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public static GameLanguages CurrentLanguage;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        EventBus.Subscribe<ChangePanelEvent>(OnChangePanel);
        EventBus.Subscribe<GameLanguageChangeEvent>(OnChangeLanguage);
        EventBus.Subscribe<ActivateSubPanelEvent>(OnActivateSubPanel);
        EventBus.Subscribe<ToggleSubPanelEvent>(OnToggleSubPanel);
        //Substitute the language arbitrarly selection after Save System implementation
        EventBus.Publish(new GameLanguageChangeEvent(GameLanguages.English));
    }
    static void OnChangePanel(ChangePanelEvent eventData)
    {
        eventData.currentPanel.OnExit(eventData.currentPanel);
        eventData.targetPanel.OnEnter(eventData.targetPanel);
    }

    static void OnActivateSubPanel(ActivateSubPanelEvent eventData)
    {
        eventData.subPanel.OnEnter(eventData.subPanel);
        foreach (var panel in eventData.otherSubPanels)
        {
            panel.OnExit(panel);
        }
    }

    static void OnToggleSubPanel(ToggleSubPanelEvent eventData)
    {
        if (eventData.activate)
        {
            eventData.subPanel.OnEnter(eventData.subPanel);
        }
        else
        {
            eventData.subPanel.OnExit(eventData.subPanel);
        }
    }
    static void OnChangeLanguage(GameLanguageChangeEvent eventData)
    {
        CurrentLanguage = eventData.CurrentLanguage;
    }
}
