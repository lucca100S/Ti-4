using UnityEngine;

public class CloseApplicationButton : UIComponentBase 
{ 
    public override void OnTrigger()
    {
        base.OnTrigger();
        EventBus.Publish(new EndApplicationEvent { });
    }
}
