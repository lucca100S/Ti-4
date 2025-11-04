using UnityEngine;

public abstract class UIComponentBase : MonoBehaviour
{
    [Tooltip("Nome único do componente (opcional, para debug ou eventos).")]
    public string componentName;

    public virtual void OnTrigger()
    {
        EventBus.Publish(new ComponentTriggeredEvent { ComponentName = componentName });
    }
}