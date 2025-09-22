using UnityEngine;

/// <summary>
/// Representa um checkpoint interagível no cenário.
/// </summary>
public class CheckPoint : OptionalInteractableObjects
{
    [SerializeField]
    GameObject _spawnPoint;

    public override void Interaction()
    {
        #if UNITY_EDITOR
        base.Interaction();
        #endif
        /* Lógica de checkpoint */
    }
}
