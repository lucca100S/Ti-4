using UnityEngine;

/// <summary>
/// Representa um checkpoint interag�vel no cen�rio.
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
