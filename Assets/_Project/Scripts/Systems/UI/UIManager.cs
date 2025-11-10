using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("Lista de painéis disponíveis na cena.")]
    public List<GameObject> panels = new();

    private void OnEnable()
    {
        EventBus.Subscribe<SceneChangeEvent>(OnSceneChange);
        EventBus.Subscribe<GameObjectRevealButton>(OnPanelToggle);
        EventBus.Subscribe<ComponentTriggeredEvent>(OnComponentTrigger);
        EventBus.Subscribe<EndApplicationEvent>(evt => OnCloseApplication());
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<SceneChangeEvent>(OnSceneChange);
        EventBus.Unsubscribe<GameObjectRevealButton>(OnPanelToggle);
        EventBus.Unsubscribe<ComponentTriggeredEvent>(OnComponentTrigger);
        EventBus.Unsubscribe<EndApplicationEvent>(evt => OnCloseApplication());
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
        Debug.Log("Fechando a aplicação...");
        Application.Quit();
    }

    private void OnComponentTrigger(ComponentTriggeredEvent evt)
    {
        Debug.Log($"Componente '{evt.ComponentName}' utilizado!");
    }
}
