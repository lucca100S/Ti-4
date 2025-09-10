using System;
using UnityEngine;
/// <summary>
/// Define um colet�vel
/// </summary>
[RequireComponent(typeof(Collider))]
public class Collectable : InteractableObjects
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

            // Desativa o objeto para evitar m�ltiplas intera��es
            gameObject.SetActive(false);
        }
    }
}
