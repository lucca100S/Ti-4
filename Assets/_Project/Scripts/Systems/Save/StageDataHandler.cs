using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sistema central de dados de estágio.
/// Pode ser anexado em qualquer cena.
/// Detecta automaticamente checkpoints e coletáveis na cena.
/// </summary>
[ExecuteAlways]
public class StageDataHandler : MonoBehaviour
{
    [Header("Dados de Estágios")]
    public List<StageEntry> stageEntries = new();

    [Header("Estágio Atual (Auto-Detectado)")]
    [SerializeField] private StageData currentStageData;

    private void OnEnable()
    {
        AutoFillStageData();
    }

    /// <summary>
    /// Detecta automaticamente os dados do estágio da cena atual.
    /// </summary>
    private void AutoFillStageData()
    {
        string currentStageKey = SceneManager.GetActiveScene().name;

        // Busca todos os componentes na cena
        var checkpoints = new List<CheckPoint>(FindObjectsByType<CheckPoint>(FindObjectsSortMode.None));
        var collectables = new List<Collectables>(FindObjectsByType<Collectables>(FindObjectsSortMode.None));

        // Cria o StageData preenchendo com os structs internos
        StageData generatedData = new StageData
        {
            StageName = currentStageKey,
            checkpoints = new List<CheckpointData>(),
            collectables = new List<CollectableData>()
        };

        // Extrai os structs de dentro dos componentes
        foreach (var cp in checkpoints)
        {
            generatedData.checkpoints.Add(cp.checkpointData); // ou cp.data, dependendo do nome do campo
        }

        foreach (var c in collectables)
        {
            generatedData.collectables.Add(c.collectableData); // idem
        }

        // Verifica se já existe entry para essa cena
        int index = stageEntries.FindIndex(e => e.stageKey.Equals(currentStageKey, StringComparison.OrdinalIgnoreCase));

        if (index >= 0)
        {
            stageEntries[index] = new StageEntry
            {
                stageKey = currentStageKey,
                stageData = generatedData
            };
        }
        else
        {
            stageEntries.Add(new StageEntry
            {
                stageKey = currentStageKey,
                stageData = generatedData
            });
        }

        currentStageData = generatedData;

        Debug.Log($"[StageDataSystem] Dados da cena '{currentStageKey}' atualizados automaticamente. " +
                  $"Checkpoints: {generatedData.checkpoints.Count}, Coletáveis: {generatedData.collectables.Count}");
    }
    /// <summary>
    /// Atualiza os dados de um CheckPoint ou Collectable específico.
    /// Pode ser chamado diretamente por esses componentes.
    /// </summary>
    public void UpdateData(CheckPoint checkPoint)
    {
        for (int i = 0; i < currentStageData.checkpoints.Count; i++)
        {
            if (currentStageData.checkpoints[i].ID == checkPoint.checkpointData.ID)
            {
                currentStageData.checkpoints[i] = checkPoint.checkpointData;
                Debug.Log($"[StageDataSystem] CheckPoint '{checkPoint.checkpointData.ID}' atualizado.");
                return;
            }
        }

        Debug.LogWarning($"[StageDataSystem] CheckPoint '{checkPoint.checkpointData.ID}' não encontrado nos dados atuais.");
    }

    public void UpdateData(Collectables collectable)
    {
        for (int i = 0; i < currentStageData.collectables.Count; i++)
        {
            if (currentStageData.collectables[i].ID == collectable.collectableData.ID)
            {
                currentStageData.collectables[i] = collectable.collectableData;
                Debug.Log($"[StageDataSystem] Coletável '{collectable.collectableData.ID}' atualizado.");
                return;
            }
        }

        Debug.LogWarning($"[StageDataSystem] Coletável '{collectable.collectableData.ID}' não encontrado nos dados atuais.");
    }
    /// <summary>
    /// Retorna os dados do estágio atual.
    /// </summary>
    public StageData GetCurrentStageData() => currentStageData;
}
