using UnityEngine;

public class SceneButton : UIComponentBase
{
    [Tooltip("Nome da cena a ser carregada.")]
    public string targetScene;

    public override void OnTrigger()
    {
        base.OnTrigger();
        EventBus.Publish(new SceneChangeEvent { SceneName = targetScene });
    }
}

