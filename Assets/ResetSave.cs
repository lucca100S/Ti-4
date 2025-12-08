using UnityEngine;

public class ResetSave : Button
{
    public override void OnClik()
    {
        SaveHandler.ResetAllSaves();
    }
}
