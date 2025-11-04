using UnityEngine;

public class PanelButton : UIComponentBase
{
    [Tooltip("Nome do painel que será ativado/desativado.")]
    public string targetPanel;
    [Tooltip("Define se o painel será ativado ou desativado.")]
    public bool activate;

    public override void OnTrigger()
    {
        base.OnTrigger();
        EventBus.Publish(new PanelToggleEvent
        {
            PanelName = targetPanel,
            Active = activate
        });
    }
}
