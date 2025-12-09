using UnityEngine;

public class ResetSave : Button
{
    public override void OnClik()
    {
        CountCollectables.ResetAll();
    }
}
