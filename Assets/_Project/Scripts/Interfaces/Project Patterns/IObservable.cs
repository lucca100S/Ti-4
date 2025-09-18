using System;
using System.Collections.Generic;

/// <summary>
/// Contrato para qualquer objeto observável que dispare eventos do tipo T.
/// </summary>
public interface IObservable<T>
{
    List<Action<T>> Observers { get; set; }
    void SubscribeListeners(IObserver<T> observer);
    void UnsubscribeListeners(IObserver<T> observer);
    void NotifyListeners(T item);
}
