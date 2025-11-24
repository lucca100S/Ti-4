using UnityEngine;

public class ChangePanelButton : Button
{
    public ChangePanelEvent changePanelEvent;
    public override void OnClik()
    {
        EventBus.Publish(new ChangePanelEvent(changePanelEvent.currentPanel, changePanelEvent.targetPanel));
    }
}

