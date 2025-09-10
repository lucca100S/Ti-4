using System;
using UnityEngine;
/// <summary>
/// Defini o básico de um checkpoint
/// </summary>
public class CheckPoint : VoluntaryInteractable
{
    public override void Interaction()
    {
        if (base.IsInteractable()) 
        {
            //Completar com a rotina de alocação do CheckPoint no player, e dar play na animação/VFX, relacionados ao checkpoint
        }
    }
}
