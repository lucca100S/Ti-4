using System;
using UnityEngine;

/// <summary>
/// Representa um checkpoint interagível no cenário.
/// </summary>
public class CheckPoint : OptionalInteractableObjects
{
    public GameObject spawnPoint;
    public CheckpointData checkpointData;
    public override void Interaction()
    {
        if (!checkpointData.Activated) 
        {
            FindFirstObjectByType<PlayerSpawnpoint>().SetSpawnPoint(spawnPoint.transform.position);
            this.GetComponent<Renderer>().material.color = Color.red;
            checkpointData.Activated = true;
            FindAnyObjectByType<StageDataHandler>().UpdateData(this);
        }
    }
}

[Serializable]
public struct CheckpointData
{
    public string ID;    
    public bool Activated;
}