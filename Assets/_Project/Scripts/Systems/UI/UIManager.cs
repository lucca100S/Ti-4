using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using NUnit.Framework;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public static GameLanguages CurrentLanguage;
    public Pause pause;
    public MainMenu mainMenu;
    public bool isGamePaused = false;
    public GameObject volume;
    CinemachineInputAxisController cinemachineInput;
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
        EventBus.Subscribe<OpenSectionEvent>(OpenSection);
        EventBus.Subscribe<CloseSectionEvent>(CloseSection);
        //Substitute the language arbitrarly selection after Save System implementation
        EventBus.Publish(new GameLanguageChangeEvent(GameLanguages.English));
    }
    public void PauseGame()
    {
        Debug.Log("Pausing Game");
        if (!pause.isActiveAndEnabled && !isGamePaused)
        {
            pause.gameObject.SetActive(!pause.isActiveAndEnabled);
            isGamePaused = true;
            AbrirMenu();
        }
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming Game");
        if (pause.isActiveAndEnabled || mainMenu.isActiveAndEnabled && isGamePaused)
        {
            pause.gameObject.SetActive(!pause.isActiveAndEnabled);
            isGamePaused = false;
            FecharMenu();
        }
    }
    void Start()
    {
        cinemachineInput = FindAnyObjectByType<CinemachineInputAxisController>();
    }

    public void AbrirMenu()
    {
        cinemachineInput.enabled = false;     // Para a câmera de capturar o mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        volume.SetActive(true);
    }

    public void FecharMenu()
    {
        Time.timeScale = 1f;
        cinemachineInput.enabled = true;      // Reativa a câmera
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        volume.SetActive(false);
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

    static void OpenSection(OpenSectionEvent eventData)
    {
        Debug.Log("Opening Section: " + eventData.section.name);
        eventData.section.SetActive(true);
        foreach (var section in eventData.sectionsToClose)
        {
            section.SetActive(false);
        }
    }

    static void CloseSection(CloseSectionEvent eventData)
    {
        foreach (var section in eventData.section)
        {
            Debug.Log("Closing Section: " + section.name);
            section.SetActive(false);
        }
    }
}
