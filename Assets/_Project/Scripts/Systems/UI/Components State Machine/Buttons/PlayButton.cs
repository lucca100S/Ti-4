using UnityEngine;

public class PlayButton : Button
{
    public override void OnClik()
    {
        FindAnyObjectByType<PlayerStartAnimationHandler>().StartAnimation();
        FindAnyObjectByType<MainMenu>().OnExit(FindAnyObjectByType<HUD>());
    }
}
