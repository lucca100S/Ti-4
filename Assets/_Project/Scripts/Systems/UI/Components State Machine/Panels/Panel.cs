using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public abstract class Panel : UIComponent, UICompleteStateMachineInterafce
{
    public GameObject firstSelected;
    void Awake()
    {
        EventBus.Subscribe<DeviceChangeEvent>(OnDeviceChange);
    }
    void OnEnable()
    {
        if (InputDeviceDetector.UsingGamepad)
        {
            EventBus.Publish(new DeviceChangeEvent { isUsingGamepad = true } );
        }  
    }

    public abstract void OnEnter(Panel previous);
    public abstract void OnExit(Panel next);
    private void OnDeviceChange(DeviceChangeEvent deviceChangeEvent)
    {
        if (deviceChangeEvent.isUsingGamepad && this.isActiveAndEnabled && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}
