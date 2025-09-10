using System;
using UnityEngine;
/// <summary>
/// Contrato que define um objeto que pode sofrer interação
/// </summary>
public interface IInteractable
{
    Func<bool> IsInteractable { get; }
    void Interaction();
}


