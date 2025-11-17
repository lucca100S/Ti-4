using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class UIManager : MonoBehaviour
{
    public GameObject volume;
    private void OnEnable()
    {
        EventBus.Subscribe<SceneChangeEvent>(OnSceneChange);
        EventBus.Subscribe<GameObjectRevealButton>(OnPanelToggle);
        EventBus.Subscribe<ComponentTriggeredEvent>(OnComponentTrigger);
        EventBus.Subscribe<EndApplicationEvent>(evt => OnCloseApplication());
        EventBus.Subscribe<SelectButtonEvent>(SelectButton);
    }

    void Update()
    {
        // Verifica o botão Escape do teclado
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            this.GetComponent<TogglePanelButton>().OnTrigger();
            volume.SetActive(!this.GetComponent<TogglePanelButton>().activate);
        }
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<SceneChangeEvent>(OnSceneChange);
        EventBus.Unsubscribe<GameObjectRevealButton>(OnPanelToggle);
        EventBus.Unsubscribe<ComponentTriggeredEvent>(OnComponentTrigger);
        EventBus.Unsubscribe<EndApplicationEvent>(evt => OnCloseApplication());
        EventBus.Unsubscribe<SelectButtonEvent>(SelectButton);
    }

    private void SelectButton(SelectButtonEvent evt)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(evt.obj);
    }
    private void OnSceneChange(SceneChangeEvent evt)
    {
        Debug.Log($"Mudando para a cena: {evt.SceneName}");
        SceneManager.LoadScene(evt.SceneName);
    }

    private void OnPanelToggle(GameObjectRevealButton evt)
    {
        if (evt.GameObject == null)
        {
            Debug.LogWarning("Evento GameObjectRevealButton recebido com Panel nulo.");
            return;
        }
        if (evt.HideOthers.Count > 0)
        {
            foreach (var panel in evt.HideOthers)
            {
                panel.SetActive(false);
            }
        }
        evt.GameObject.SetActive(evt.Active);
        Debug.Log($"Painel {evt.GameObject.name} -> {(evt.Active ? "Ativado" : "Desativado")}");
    }

    private void OnCloseApplication()
    {
        Debug.Log("Fechando a aplica��o...");
        Application.Quit();
    }

    private void OnComponentTrigger(ComponentTriggeredEvent evt)
    {
        Debug.Log($"Componente '{evt.ComponentName}' utilizado!");
    }
}
