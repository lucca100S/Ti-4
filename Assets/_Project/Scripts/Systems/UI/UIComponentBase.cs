using UnityEngine;

public abstract class UIComponentBase : MonoBehaviour
{
    public virtual void OnTrigger()
    {
        EventBus.Publish(new ComponentTriggeredEvent { ComponentName = this.gameObject.name });
    }
}