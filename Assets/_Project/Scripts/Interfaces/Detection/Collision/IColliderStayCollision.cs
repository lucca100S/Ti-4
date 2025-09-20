using UnityEngine;

/// <summary>
/// Contrato para eventos de permanência em trigger.
/// Herdando ICollisionFilterDetection para permitir filtros.
/// </summary>
public interface IColliderStayCollision : ICollisionFilterDetection
{
    void OnTriggerStay(Collider other);
}
