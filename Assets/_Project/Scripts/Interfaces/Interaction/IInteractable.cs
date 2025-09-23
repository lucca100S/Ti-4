using System;
using UnityEngine;

/// <summary>
/// Contrato para qualquer objeto que possa ser interagido.
/// Define uma função que retorna se está interagível e um método de interação.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Define se um objeto sofreu interação previamente
    /// </summary>
    bool PreviouslyInteracted { get; set; }

    /// <summary>
    /// Define o comportamento do objeto quando pode ser interagido
    /// </summary>
    void OnIsIntereactable(); 

    /// <summary>
    /// Define o comportamento do objeto quando não pode ser interagido
    /// </summary>
    void OnIsNotIntereactable();

    /// <summary>
    /// Lógica de interação quando o jogador ou sistema chama o evento.
    /// </summary>
    void Interaction();

    /// <summary>
    /// Lógica de detecção para detectar se esta interativo ou não e inseri-lo e remove-lo da lista de interativos
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other);
    void OnTriggerExit(Collider other);
}
