using UnityEngine;
using UnityEngine.EventSystems;

public class LeavesHoover : UIComponentBase, IPointerEnterHandler, IPointerExitHandler
{
    bool active;
    public override void OnTrigger()
    {
        base.OnTrigger();
        EventBus.Publish(
            new LeavesHooverEfectMainMenu
            {
                active = active,
                position = new Vector2(this.transform.position.x, this.transform.position.y + 47)
            }
            );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        active = true;
        OnTrigger();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        active = false;
        OnTrigger();
    }
}
