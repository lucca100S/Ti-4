using System;
using System.Collections.Generic;
using UnityEngine;
using Lugu.Utils;

/// <summary>
/// Classe observável para coletáveis, usando Singleton.
/// Compatível com C# 9.0.
/// </summary>
public class CollectableObservable : SingletonMono<CollectableObservable>, IObservable<Collectables>
{

    // Lista dos observers que serão notificados
    public List<Action<Collectables>> Observers { get; set; } = new();

    // Mapeia cada observer ao delegate que criamos para ele
    private readonly Dictionary<IObserver<Collectables>, Action<Collectables>> _observerMap = new();


    public void SubscribeListeners(IObserver<Collectables> observer)
    {
        if (_observerMap.ContainsKey(observer))
            return; // já está inscrito

        // Criamos um delegate explicitamente
        Action<Collectables> callback = (item) => observer.OnCollectable(item);

        _observerMap[observer] = callback;
        Observers.Add(callback);
    }

    public void UnsubscribeListeners(IObserver<Collectables> observer)
    {
        if (!_observerMap.TryGetValue(observer, out var callback))
            return; // não está inscrito

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
