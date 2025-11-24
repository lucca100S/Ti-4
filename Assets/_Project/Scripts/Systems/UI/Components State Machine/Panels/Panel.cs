using UnityEngine;

public abstract class Panel : UIComponent, UICompleteStateMachineInterafce
{
    public abstract void OnEnter(Panel previous);
    public abstract void OnExit(Panel next);
}
