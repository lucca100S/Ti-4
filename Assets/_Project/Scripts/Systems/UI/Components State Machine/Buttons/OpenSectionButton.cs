using UnityEngine;

public class OpenSectionButton : Button
{
    public OpenSectionEvent openSectionEvent;
    public override void OnClik()
    {
        EventBus.Publish(new OpenSectionEvent(openSectionEvent.section, openSectionEvent.sectionsToClose));
    }
}