using UnityEngine;

/// <summary>
/// Define o comportamento geral dos Obstacles das fases
/// </summary>
public class Obstacles : NonVoluntaryInteractable
{
    public override void Interaction()
    {
        if (IsInteractable())
        {
            // Disparar evento no player
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Interaction(); // chama a interação automaticamente
        }
    }
}
