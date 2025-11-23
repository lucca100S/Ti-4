using UnityEngine;

public class ActivateSubPanelButton : Button
{
    public ActivateSubPanelEvent subPanelEvent;
    public override void OnClik()
    {
        EventBus.Publish(new ActivateSubPanelEvent(subPanelEvent.subPanel, subPanelEvent.otherSubPanels));
    }
}
