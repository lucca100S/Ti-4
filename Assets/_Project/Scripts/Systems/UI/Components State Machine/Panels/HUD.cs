using UnityEngine;

public class HUD : Panel
{
    public override void OnEnter(Panel previous)
    {
        //throw new System.NotImplementedException();
        switch (previous)
        {
            case Pause:
                Debug.Log("Resumed from Pause");
                var FadeTransition = new FadeTransition(0.5f);
                FadeTransition.PlayEnter(this, () =>
                {
                    Debug.Log("Exited to HUD");
                });
                break;
        }
        Debug.Log("HUD OnEnter");
    }

    public override void OnExit(Panel next)
    {
        // throw new System.NotImplementedException();
    }
}
