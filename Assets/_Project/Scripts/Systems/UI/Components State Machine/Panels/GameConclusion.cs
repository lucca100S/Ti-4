using UnityEngine;

public class GameConclusion : Panel
{
    public override void OnEnter(Panel previous)
    {
        this.gameObject.SetActive(true);
    }

    public override void OnExit(Panel next)
    {
        this.gameObject.SetActive(false);
    }
}
