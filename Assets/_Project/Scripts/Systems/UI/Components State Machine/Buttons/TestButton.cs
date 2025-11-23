using UnityEngine;

public class TestButton : Button
{
    public override void OnClik()
    {
        EventBus.Publish(new AddCollectableCountEvent{});
    }
}
