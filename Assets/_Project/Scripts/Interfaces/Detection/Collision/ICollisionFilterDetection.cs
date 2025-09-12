using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contrato para defini��o de filtros de colis�o.
/// Permite limitar detec��o por LayerMask e Tags.
/// </summary>
public interface ICollisionFilterDetection
{
    LayerMask CollisionMask { get; set; }
    HashSet<string> CollisionTags { get; set; }
}
