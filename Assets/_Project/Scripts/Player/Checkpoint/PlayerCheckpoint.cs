using UnityEngine;

public class PlayerCheckpoint : InteractableAgent
{
    [SerializeField]
    CheckPoint currentCheckpointPosition;

    void SpawnAtCheckpoint(Vector3 pos) 
    {
        //Teste
        this.transform.position = pos;
    }

    public void SetCheckpoint(CheckPoint checkPoint) => this.currentCheckpointPosition = checkPoint;
}
