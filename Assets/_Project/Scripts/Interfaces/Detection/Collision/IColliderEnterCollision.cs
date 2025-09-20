using UnityEngine;

/// <summary>
/// Contrato para eventos de entrada de trigger.
/// Herdando ICollisionFilterDetection para permitir filtros.
/// </summary>
public interface IColliderEnterCollision : ICollisionFilterDetection
{
    void OnTriggerEnter(Collider other);
}
