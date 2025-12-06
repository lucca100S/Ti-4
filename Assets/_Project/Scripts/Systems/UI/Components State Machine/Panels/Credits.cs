using UnityEngine;

public class Credits : Panel
{
    public override void OnEnter(Panel previous)
    {
        this.gameObject.SetActive(true);
    }

    public override void OnExit(Panel next)
    {
        //base.OnExit(next);
    }
}
