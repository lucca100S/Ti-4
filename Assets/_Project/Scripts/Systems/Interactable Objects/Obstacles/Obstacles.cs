using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe base para obst�culos que detectam colis�es via Trigger.
/// Fornece implementa��o b�sica de filtros de colis�o.
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
