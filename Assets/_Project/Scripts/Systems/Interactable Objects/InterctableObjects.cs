using System;
using UnityEngine;

/// <summary>
/// Classe abstrata que usa o contrato IInteractable e define um padrão na condição de interação 
/// </summary>
public abstract class InteractableObjects : MonoBehaviour, IInteractable
{
    [Header("Detection")]
    public float radius = 3f;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected string targetTag;
    public virtual Func<bool> IsInteractable => throw new NotImplementedException();

    public abstract void Interaction();
}