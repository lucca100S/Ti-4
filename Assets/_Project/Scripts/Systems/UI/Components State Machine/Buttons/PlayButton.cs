using UnityEngine;

public class PlayButton : Button
{
    public HUD hud;
    public override void OnClik()
    {
        FindAnyObjectByType<PlayerStartAnimationHandler>().StartAnimation();
        FindAnyObjectByType<MainMenu>().OnExit(FindAnyObjectByType<HUD>());
        hud.OnEnter(FindAnyObjectByType<MainMenu>());
    }
}
