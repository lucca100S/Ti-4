using System.Collections.Generic;
using UnityEngine;

public class FillBackButton : Button
{
    public BackButton backButton;
    public GameObject target;
    public override void OnClik()
    {
        backButton.target = target;
    }
}
