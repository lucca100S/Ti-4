using System.Collections.Generic;

/// <summary>
/// Contrato para qualquer objeto que observe eventos do tipo T.
/// </summary>
public interface IObserver<T>
{
    void OnCollectable(T item);
    void SetCollectedCollectables(List<T> itens);
}
