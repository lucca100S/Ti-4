using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputDeviceDetector : MonoBehaviour
{
    public static bool UsingGamepad = false;

    bool lastState = false; // false = mouse/keyboard | true = gamepad

    void Update()
    {
        bool usingGamepadNow =
            Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame;

        bool usingKeyboardMouseNow =
            Mouse.current != null && Mouse.current.wasUpdatedThisFrame
            || Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame;

        // detecta troca para gamepad
        if (!lastState && usingGamepadNow)
        {
            lastState = true;
            UsingGamepad = true;
            EventBus.Publish(new DeviceChangeEvent { isUsingGamepad = true });
        }

        // detecta troca para mouse/teclado
        if (lastState && usingKeyboardMouseNow)
        {
            lastState = false;
            UsingGamepad = false;
            EventBus.Publish(new DeviceChangeEvent { isUsingGamepad = false });
        }
    }

}
