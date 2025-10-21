using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject checkpoint;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerSpawnpoint>().SetSpawnPoint(checkpoint.transform.position);
        Debug.Log("Colission");
    }
}
