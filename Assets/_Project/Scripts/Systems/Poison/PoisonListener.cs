using UnityEngine;
using UnityEngine.Events;

public class PoisonListener : MonoBehaviour
{
    [SerializeField] private UnityEvent _cleanTriggerEvent;
    [SerializeField] private UnityEvent _cleanStartEvent;
    [SerializeField] private UnityEvent _cleanEndEvent;

    public void CleanTrigger()
    {
        _cleanTriggerEvent.Invoke();
    }

    public void CleanStart()
    {
        _cleanStartEvent.Invoke();
    }

    public void CleanEnd()
    {
        _cleanEndEvent.Invoke();
    }
}
