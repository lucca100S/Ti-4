using System;
using System.Collections.Generic;
using UnityEngine;
using Lugu.Utils;

/// <summary>
/// Classe observ�vel para colet�veis, usando Singleton.
/// Compat�vel com C# 9.0.
/// </summary>
public class CollectableObservable : SingletonMono<CollectableObservable>, IObservable<Collectables>
{

    // Lista dos observers que ser�o notificados
    public List<Action<Collectables>> Observers { get; set; } = new();

    // Mapeia cada observer ao delegate que criamos para ele
    private readonly Dictionary<IObserver<Collectables>, Action<Collectables>> _observerMap = new();


    public void SubscribeListeners(IObserver<Collectables> observer)
    {
        if (_observerMap.ContainsKey(observer))
            return; // j� est� inscrito

        // Criamos um delegate explicitamente
        Action<Collectables> callback = (item) => observer.OnCollectable(item);

        _observerMap[observer] = callback;
        Observers.Add(callback);
    }

    public void UnsubscribeListeners(IObserver<Collectables> observer)
    {
        if (!_observerMap.TryGetValue(observer, out var callback))
            return; // n�o est� inscrito

        Observers.Remove(callback);
        _observerMap.Remove(observer);
    }

    public void NotifyListeners(Collectables collectable)
    {
        Debug.Log("NotifyListeners()");
        foreach (var observer in Observers) 
        {
            Debug.Log(observer.ToString());
            observer?.Invoke(collectable);
        }
    }
}
