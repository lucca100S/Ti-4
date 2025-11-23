using UnityEngine;

public class HUD : Panel
{
    public override void OnEnter(Panel previous)
    {
        Debug.Log("HUD OnEnter");
        this.gameObject.SetActive(true);
    }

    public override void OnExit(Panel next)
    {
        // throw new System.NotImplementedException();
    }
}
