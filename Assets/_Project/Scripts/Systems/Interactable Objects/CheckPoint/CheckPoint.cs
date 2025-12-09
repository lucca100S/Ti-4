using UnityEngine;

/// <summary>
/// Representa um checkpoint interag�vel no cen�rio.
/// </summary>
public class CheckPoint : OptionalInteractableObjects
{
    public GameObject spawnPoint;
    public CheckPointSaveData checkPointSaveData;
    public override void Interaction()
    {
         Debug.Log($"<color=yellow>[CheckPoint]</color> CheckPoint ativado: {this.gameObject.name}");
        FindFirstObjectByType<PlayerSpawnpoint>().SetSpawnPoint(spawnPoint.transform.position);
        checkPointSaveData.isActivated = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !checkPointSaveData.isActivated)
        {
            Interaction();
            //FindAnyObjectByType<SaveHandler>()?.SaveSceneNow();
        }
    }
}
