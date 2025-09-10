using System;
using UnityEngine;
/// <summary>
/// Define um coletável
/// </summary>
public class Collectable : NonVoluntaryInteractable
{
    [Header("Collectable Data")]
    [SerializeField] private CollectableData data;

    // Evento global que o Observer vai escutar
    public static event Action<Collectable> OnCollected;

    public override void Interaction()
    {
        if (IsInteractable())
        {
            // Dispara evento para qualquer listener
            OnCollected?.Invoke(this);

            // Desativa o objeto para evitar múltiplas interações
            gameObject.SetActive(false);
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
