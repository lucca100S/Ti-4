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
        EventBus.Subscribe<PanelToggleEvent>(OnPanelToggle);
        EventBus.Subscribe<ComponentTriggeredEvent>(OnComponentTrigger);
        EventBus.Subscribe<EndApplicationEvent>(evt => OnCloseApplication());
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<SceneChangeEvent>(OnSceneChange);
        EventBus.Unsubscribe<PanelToggleEvent>(OnPanelToggle);
        EventBus.Unsubscribe<ComponentTriggeredEvent>(OnComponentTrigger);
        EventBus.Unsubscribe<EndApplicationEvent>(evt => OnCloseApplication());
    }

    private void OnSceneChange(SceneChangeEvent evt)
    {
        Debug.Log($"Mudando para a cena: {evt.SceneName}");
        SceneManager.LoadScene(evt.SceneName);
    }

    private void OnPanelToggle(PanelToggleEvent evt)
    {
        bool found = false;

        foreach (var panel in panels)
        {
            // Se o nome não for o painel alvo, desativa
            if (panel.name != evt.PanelName)
            {
                panel.SetActive(false);
                continue;
            }

            // Caso contrário, ativa ou desativa conforme o evento
            panel.SetActive(evt.Active);
            Debug.Log($"Painel {evt.PanelName} -> {(evt.Active ? "Ativado" : "Desativado")}");
            found = true;
        }

        // Caso nenhum painel tenha o nome especificado
        if (!found)
            Debug.LogWarning($"Painel '{evt.PanelName}' não encontrado na lista do UIManager.");
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