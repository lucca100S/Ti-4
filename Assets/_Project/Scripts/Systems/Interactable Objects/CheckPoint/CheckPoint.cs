using System;
using UnityEngine;
/// <summary>
/// Defini o b�sico de um checkpoint
/// </summary>
public class CheckPoint : VoluntaryInteractable
{
    public override void Interaction()
    {
        if (base.IsInteractable()) 
        {
            //Completar com a rotina de aloca��o do CheckPoint no player, e dar play na anima��o/VFX, relacionados ao checkpoint
        }
    }
}
