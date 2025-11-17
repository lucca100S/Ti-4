using UnityEngine;

public class SetSelectedButton: UIComponentBase
{
    public GameObject selected;
    public override void OnTrigger()
    {
        base.OnTrigger();
        EventBus.Publish<SelectButtonEvent>(new SelectButtonEvent { obj = selected });
    }
}
