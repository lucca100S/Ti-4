using UnityEngine;

public class CloseSectionButton : Button
{
    public CloseSectionEvent closeSectionEvent;
    public override void OnClik()
    {
        EventBus.Publish(new CloseSectionEvent(closeSectionEvent.section));
    }
}