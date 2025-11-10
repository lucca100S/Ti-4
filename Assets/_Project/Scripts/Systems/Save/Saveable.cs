using System;
using System.Collections.Generic;
using UnityEngine;

#region ======== DATA STRUCTURES ========
/// <summary>
/// Conjunto completo de dados referentes a um estágio.
/// </summary>
[Serializable]
public class StageData
{
    public string StageName;
    public List<CheckpointData> checkpoints = new();
    public List<CollectableData> collectables = new();

    public StageData() { }

    public StageData(string stageName)
    {
        StageName = stageName;
    }
}

/// <summary>
/// Estrutura usada pelo StageDataSystem para indexar estágios por chave.
/// </summary>
[Serializable]
public struct StageEntry
{
    public string stageKey;
    public StageData stageData;

    public StageEntry(string key, StageData data)
    {
        stageKey = key;
        stageData = data;
    }
}

#endregion
