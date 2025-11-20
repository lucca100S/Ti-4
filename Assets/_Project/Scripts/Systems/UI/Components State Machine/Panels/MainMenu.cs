using UnityEngine;

public class MainMenu : Panel
{
    public override void OnEnter(Panel previous)
    {
        Debug.Log("Main Menu specific OnEnter logic");
    }
    public override void OnExit(Panel next)
    {
        Debug.Log("Main Menu specific OnExit logic");
    }
}
