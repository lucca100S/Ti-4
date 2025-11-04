using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "UI Actions/Change Scene")]
public class ChangeSceneAction : ButtonAction
{
    public string sceneName;
    public override void Execute()
    {
        SceneManager.LoadScene(sceneName);
    }
}
