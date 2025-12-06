using System.Collections.Generic;
using UnityEngine;

public class BackButton : Button
{
    public GameObject target;
    public List<GameObject> toDisable;
    public override void OnClik()
    {
        EventBus.Publish(new OpenSectionEvent(target, toDisable));
    }
}
