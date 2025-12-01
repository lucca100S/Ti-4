using UnityEngine;

public class ToggleSubPanel : Button
{
    public ToggleSubPanelEvent subPanelEvent;
    public override void OnClik()
    {
        EventBus.Publish(new ToggleSubPanelEvent(subPanelEvent.subPanel, subPanelEvent.activate));
        subPanelEvent.activate = !subPanelEvent.activate;
    }
}

