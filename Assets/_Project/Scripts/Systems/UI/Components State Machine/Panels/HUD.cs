using UnityEngine;

public class HUD : Panel
{
    public override void OnEnter(Panel previous)
    {
        //throw new System.NotImplementedException();
        Debug.Log("HUD OnEnter");
    }

    public override void OnExit(Panel next)
    {
       // throw new System.NotImplementedException();
    }
}
