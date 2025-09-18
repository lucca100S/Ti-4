using System;

/// <summary>
/// Contrato para qualquer objeto que possa ser interagido.
/// Define uma fun��o que retorna se est� interag�vel e um m�todo de intera��o.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Fun��o que indica se o objeto est� em estado de ser interagido.
    /// </summary>
    Func<bool> IsInteractable { get; }

    /// <summary>
    /// L�gica de intera��o quando o jogador ou sistema chama o evento.
    /// </summary>
    void Interaction();
}
