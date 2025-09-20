using System;

/// <summary>
/// Contrato para qualquer objeto que possa ser interagido.
/// Define uma função que retorna se está interagível e um método de interação.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Função que indica se o objeto está em estado de ser interagido.
    /// </summary>
    Func<bool> IsInteractable { get; }

    /// <summary>
    /// Lógica de interação quando o jogador ou sistema chama o evento.
    /// </summary>
    void Interaction();
}
