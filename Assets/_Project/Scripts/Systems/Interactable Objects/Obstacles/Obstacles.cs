using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe base para obstáculos que detectam colisões via Trigger.
/// </summary>
public class Obstacles : MonoBehaviour, IColliderEnterCollision
{
    public HashSet<string> CollisionTags { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (CollisionTags.Contains(other.tag))
            {
                //Rotina de "Morte"
            }
        }
    }
}
