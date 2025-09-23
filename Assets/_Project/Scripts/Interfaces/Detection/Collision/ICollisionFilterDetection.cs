using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contrato para definição de filtros de colisão.
/// Permite limitar detecção por LayerMask e Tags.
/// </summary>
public interface ICollisionFilterDetection
{
    HashSet<string> CollisionTags { get; set; }
}
