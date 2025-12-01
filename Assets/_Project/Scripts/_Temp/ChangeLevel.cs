using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public string targetScene;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventBus.ClearAll();
            SceneManager.LoadScene(targetScene);
        }
    }
}
