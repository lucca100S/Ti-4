using System;
using System.Collections.Generic;
using UnityEngine;

public enum PanelNames
{
    Settings, Collectables, Credits, Defeated, Loading, PUC_Logo, Main_Menu, Pause, Saving, HUD, Error
}


public static class PanelNamesExtensions
{
    //Pattern: PanelNames_Panel
    public static string ToPanelName(this PanelNames panelName)
    {
        if (panelName == PanelNames.Error)
        {
            Debug.LogError($"Function called with {panelName}");
            return string.Empty;
        }
        return panelName.ToString() + "_Panel";
    }

    public static PanelNames ToPanelName(string panelName) 
    {
        string panelNameString = panelName.Split('_')[0];
        foreach(PanelNames panelNames in Enum.GetValues(typeof(PanelNames)))
        { 
            if(panelNameString.CompareTo(panelNames.ToString()) == 0)
            {
                return panelNames;                
            }
        }
        return PanelNames.Error;
    }
}

