using System.Collections.Generic;
using UnityEngine;

public enum PanelNames
{
    Settings, Collectables, Credits, Defeated, Loading, PUC_Logo, Main_Menu, Pause, Saving, HUD
}


public static class PanelNamesExtensions
{
    //Pattern: PanelNames_Panel
    public static string ToPanelName(this PanelNames panelName)
    {
        return panelName.ToString() + "_Panel";
    }
}

