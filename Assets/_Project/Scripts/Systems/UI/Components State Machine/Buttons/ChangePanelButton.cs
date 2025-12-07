using UnityEngine;
using UnityEngine.EventSystems;
public class ChangePanelButton : Button
{
    public ChangePanelEvent changePanelEvent;
    public override void OnClik()
    {
        EventBus.Publish(new ChangePanelEvent(changePanelEvent.currentPanel, changePanelEvent.targetPanel));
        EventSystem.current.SetSelectedGameObject(null);
    }
}

