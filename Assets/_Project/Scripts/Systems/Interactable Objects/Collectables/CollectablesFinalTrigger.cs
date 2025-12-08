using UnityEngine;

public class CollectablesFinalTrigger : MonoBehaviour
{
    public HUD hud;
    public GameConclusion gameConclusion;
    public UIManager uIManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hud.gameObject.SetActive(false);
            gameConclusion.gameObject.SetActive(true);
            uIManager.SetCursorState(true, CursorLockMode.None);
            ActionsManager.Instance.OnFinalLevelCompleted?.Invoke();
        }
    }
}
