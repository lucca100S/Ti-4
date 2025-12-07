using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public static GameLanguages CurrentLanguage;
    public Pause pause;
    public MainMenu mainMenu;
    public HUD hud;
    public List<GameObject> sections = new List<GameObject>();
    public bool isGamePaused = false;
    public GameObject volume;
    CinemachineInputAxisController cinemachineInput;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        //Lógica integrada com save para mudar estado do cursor
        SetCursorState(true, CursorLockMode.None);
        cinemachineInput = FindAnyObjectByType<CinemachineInputAxisController>();
        EventBus.Subscribe<ChangePanelEvent>(OnChangePanel);
        EventBus.Subscribe<GameLanguageChangeEvent>(OnChangeLanguage);
        EventBus.Subscribe<ActivateSubPanelEvent>(OnActivateSubPanel);
        EventBus.Subscribe<ToggleSubPanelEvent>(OnToggleSubPanel);
        EventBus.Subscribe<OpenSectionEvent>(OpenSection);
        EventBus.Subscribe<CloseSectionEvent>(CloseSection);
        //Substitute the language arbitrarly selection after Save System implementation
        EventBus.Publish(new GameLanguageChangeEvent(GameLanguages.English));
    }

    void SetCursorState(bool visible, CursorLockMode lockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = lockMode;
    }
    public void HandleEscPress()
    {
        // Se HUD (volume desligado/menus fechados) -> pausa normalmente
        if (!isGamePaused)
        {
            PauseGame();
            return;
        }
        Debug.Log("<color=yellow>[UI MANAGER]</color> Fechando menus e retornando ao jogo via ESC");
        // Desativar todos os menus
        foreach (var section in sections)
        {
            section.SetActive(false);
        }
        // Reactivar HUD (CASO mantenha referência separada, ative aqui)
        hud.gameObject.SetActive(true);
        AudioPlayer.Stop(AudioId.MenuMusic);
        AudioPlayer.Play(AudioId.InGameMusic);
        FecharMenu(); // restaura câmera, inputs e cursor
    }

    public void PauseGame()
    {
        Debug.Log($"<color=purple>[UI MANAGER]</color> Pausando jogo");
        if (!pause.isActiveAndEnabled && !isGamePaused)
        {
            pause.gameObject.SetActive(!pause.isActiveAndEnabled);
            isGamePaused = true;
            AbrirMenu();
        }
    }

    public void ResumeGame()
    {
        Debug.Log($"<color=purple>[UI MANAGER]</color> Voltando ao jogo");
        if (pause.isActiveAndEnabled || mainMenu.isActiveAndEnabled && isGamePaused)
        {
            pause.gameObject.SetActive(!pause.isActiveAndEnabled);
            FecharMenu();
        }
    }

    public void AbrirMenu()
    {
        cinemachineInput.enabled = false;     // Para a câmera de capturar o mouse
        SetCursorState(true, CursorLockMode.None);
        FindAnyObjectByType<Player.PlayerInput>().DisableMovementInputs();
        volume.SetActive(true);
    }

    public void FecharMenu()
    {
        isGamePaused = false;
        cinemachineInput.enabled = true;      // Reativa a câmera
        SetCursorState(false, CursorLockMode.Locked);
        FindAnyObjectByType<Player.PlayerInput>().EnableMovementInputs();
        EventSystem.current.SetSelectedGameObject(null);
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
