using System.Collections.Generic;
using UnityEngine;

public class PlayButton : Button
{
    public HUD hud;
    public List<GameObject> hideSections = new List<GameObject>();
    public override void OnClik()
    {
        if (!UIManager.Instance.isGamePaused)
        {
            FindAnyObjectByType<PlayerStartAnimationHandler>().StartAnimation();
            FindAnyObjectByType<MainMenu>().OnExit(FindAnyObjectByType<HUD>());
            AudioPlayer.Stop(AudioId.MenuMusic);
            AudioPlayer.Play(AudioId.InGameMusic);
        }
        else
        {
            UIManager.Instance.ResumeGame();
        }
        EventBus.Publish(new OpenSectionEvent(hud.gameObject, hideSections));
    }
}
