using UnityEngine;

/// <summary>
/// Representa um checkpoint interag�vel no cen�rio.
/// </summary>
public class CheckPoint : OptionalInteractableObjects
{
    public GameObject spawnPoint;
    public Material activatedMaterial;
    public bool activated { get; private set; }

    void Start()
    {
        activated = false;
    }

    public override void Interaction()
    {
        FindFirstObjectByType<PlayerSpawnpoint>().SetSpawnPoint(spawnPoint.transform.position);
        activated = true;
        Subject.instance.checkpointActivated?.Invoke();
        GetComponentInChildren<MeshRenderer>().material = activatedMaterial;
    }
}
