using UnityEngine;

public class CollectablesFinalTrigger : MonoBehaviour
{
    public HUD hud;
    public GameConclusion gameConclusion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hud.gameObject.SetActive(false);
            gameConclusion.gameObject.SetActive(true);
            ActionsManager.Instance.OnFinalLevelCompleted?.Invoke();
        }
    }
}
