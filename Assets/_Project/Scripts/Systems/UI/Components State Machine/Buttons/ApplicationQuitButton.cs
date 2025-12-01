using UnityEngine;

public class ApplicationQuitButton : Button
{
    public override void OnClik()
    {
        Debug.Log("Application Quit Button Clicked");
        EventBus.Publish(new ApplicationQuitEvent{});
        Application.Quit();
    }
}
