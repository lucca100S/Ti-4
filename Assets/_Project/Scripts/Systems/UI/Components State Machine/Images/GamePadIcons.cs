using System.Collections.Generic;
using UnityEngine;

public class GamePadIcons : MonoBehaviour
{
    public List<GameObject> PCIcons;
    public List<GameObject> PlayStationIcons;
    public List<GameObject> XboxIcons;

    void Awake()
    {
        EventBus.Subscribe<GamepadTypeChangeEvent>(OnGamepadTypeChange);
    }

    void OnGamepadTypeChange(GamepadTypeChangeEvent e)
    {
        // switch (InputDeviceDetector.GamepadType)
        // {
        //     case GamepadType.Xbox:
        //         SetActiveIcons(XboxIcons);
        //         break;
        //     case GamepadType.PlayStation:
        //         SetActiveIcons(PlayStationIcons);
        //         break;
        //     case GamepadType.None:
        //         SetActiveIcons(PCIcons);
        //         break;
        // }
    }

    void SetActiveIcons(List<GameObject> iconsToActivate)
    {
        foreach (var icon in PCIcons)
            icon.SetActive(false);
        foreach (var icon in PlayStationIcons)
            icon.SetActive(false);
        foreach (var icon in XboxIcons)
            icon.SetActive(false);
        foreach (var icon in iconsToActivate)
            icon.SetActive(true);
    }
}
