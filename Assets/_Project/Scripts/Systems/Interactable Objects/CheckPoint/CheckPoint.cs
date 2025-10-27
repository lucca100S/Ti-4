using UnityEngine;

/// <summary>
/// Representa um checkpoint interagível no cenário.
/// </summary>
public class CheckPoint : OptionalInteractableObjects
{
    public GameObject spawnPoint;
    public override void Interaction()
    {
        FindFirstObjectByType<PlayerSpawnpoint>().SetSpawnPoint(spawnPoint.transform.position);
        this.GetComponent<Renderer>().material.color = Color.red;
    }
}
