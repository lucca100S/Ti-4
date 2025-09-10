using System;
using UnityEngine;
/// <summary>
/// Contrato que define um objeto que pode sofrer intera��o
/// </summary>
public interface IInteractable
{
    Func<bool> IsInteractable { get; }
    void Interaction();
}


