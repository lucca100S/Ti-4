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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Interaction();
        }
    }
}
