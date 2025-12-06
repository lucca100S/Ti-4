using UnityEngine;

/// <summary>
/// Representa um checkpoint interag�vel no cen�rio.
/// </summary>
public class CheckPoint : OptionalInteractableObjects,ILoadable
{
    public GameObject spawnPoint;
    public CheckPointSaveData checkPointSaveData;
    public override void Interaction()
    {
         Debug.Log($"[CheckPoint] CheckPoint ativado: {this.gameObject.name}");
        FindFirstObjectByType<PlayerSpawnpoint>().SetSpawnPoint(spawnPoint.transform.position);
        checkPointSaveData.isActivated = true;
    }

    public void LoadData()
    {      
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !checkPointSaveData.isActivated)
        {
            Interaction();
        }
    }
}
