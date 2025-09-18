using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe base para obstáculos que detectam colisões via Trigger.
/// Fornece implementação básica de filtros de colisão.
/// </summary>
public abstract class Obstacles : MonoBehaviour, IColliderEnterCollision
{
    public LayerMask CollisionMask { get; set; }
    public HashSet<string> CollisionTags { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Chamada indevida de OnTriggerEnter em Obstacles. " +
                         "Implemente na classe concreta.");
    }
}
