using UnityEngine;

public class CollectablesFinalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionsManager.Instance.OnFinalLevelCompleted?.Invoke();
        }
    }
}
