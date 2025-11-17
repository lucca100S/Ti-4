using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class GameObjectRevealButtons : UIComponentBase
{
    [Tooltip("Nome do GO que será ativado/desativado.")]
    public GameObject target;
    [Tooltip("Define se o GO será ativado ou desativado.")]
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
    }
}
