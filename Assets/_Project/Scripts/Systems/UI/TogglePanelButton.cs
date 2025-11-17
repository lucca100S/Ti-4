using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TogglePanelButton : UIComponentBase
{
    [Tooltip("Nome do painel que será ativado/desativado.")]
    public GameObject target;
    [Tooltip("Define se o painel será ativado ou desativado.")]
    public bool activate;
    public List<GameObject> hideOthers;
    public override void OnTrigger()
    {
        base.OnTrigger();
        EventBus.Publish(new GameObjectRevealButton
        {
            GameObject = target,
            HideOthers = hideOthers,
            Active = activate
        });
        activate = !activate;   
    }
}
