using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneButton : Button
{
    public string sceneName;
    public override void OnClik()
    {
       SceneManager.LoadScene(sceneName);
    }
}
