using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIManager Instance;
    public static Dictionary<PanelNames, GameObject> panels = new Dictionary<PanelNames, GameObject>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        UIManager.FillPanelsDictionary();
        SetAllCanvasState(false);
    }

    private static void FillPanelsDictionary()
    {
        foreach (PanelNames panelName in System.Enum.GetValues(typeof(PanelNames)))
        {
            string panelObjectName = panelName.ToPanelName();
            GameObject panelObject = GameObject.Find(panelObjectName);
            if (panelObject != null)
            {
                panels[panelName] = panelObject;
                Debug.Log($"[UIManager] Painel '{panelObjectName}' adicionado ao dicionário.");
            }
            else
            {
                Debug.LogWarning($"[UIManager] Painel '{panelObjectName}' não encontrado na cena.");
            }
        }
    }

    public void SetPanelState(PanelNames panelName, bool state)
    {
        if (panels.ContainsKey(panelName))
        {
            panels[panelName].SetActive(state);
            Debug.Log($"[UIManager] Painel '{panelName.ToPanelName()}' definido para estado: {state}");
        }
        else
        {
            Debug.LogWarning($"[UIManager] Painel '{panelName.ToPanelName()}' não encontrado no dicionário.");
        }
    }

    public void SetAllCanvasState(bool state)
    {
        foreach (var panel in panels.Values)
        {
            panel.SetActive(state);
        }
        Debug.Log($"[UIManager] Todos os painéis definidos para estado: {state}");
    }
}