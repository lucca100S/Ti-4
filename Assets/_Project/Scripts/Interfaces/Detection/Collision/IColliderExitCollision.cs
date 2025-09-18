using UnityEngine;

/// <summary>
/// Contrato para eventos de sa�da de trigger.
/// Herdando ICollisionFilterDetection para permitir filtros.
/// </summary>
public interface IColliderExitCollision : ICollisionFilterDetection
{
    void OnTriggerExit(Collider other);
}
