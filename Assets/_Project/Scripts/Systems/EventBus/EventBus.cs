using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// =====================================
// EVENT BUS GLOBAL
// =====================================
public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> subscribers = new();

    // -------------------------
    // Subscribe
    // -------------------------
    public static void Subscribe<T>(Action<T> callback)
    {
        var type = typeof(T);

        if (subscribers.TryGetValue(type, out var existing))
            subscribers[type] = (Action<T>)existing + callback;
        else
            subscribers[type] = callback;
    }

    // -------------------------
    // Unsubscribe
    // -------------------------
    public static void Unsubscribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (subscribers.TryGetValue(type, out var existing))
        {
            var currentDel = (Action<T>)existing - callback;

            if (currentDel == null)
                subscribers.Remove(type);
            else
                subscribers[type] = currentDel;
        }
    }

    // -------------------------
    // Publish (com limpeza autom�tica)
    // -------------------------
    public static void Publish<T>(T message)
    {
        var type = typeof(T);

        if (!subscribers.TryGetValue(type, out var del))
            return;

        var callbacks = ((Action<T>)del).GetInvocationList();
        var validCallbacks = new List<Action<T>>(callbacks.Length);

        foreach (var cb in callbacks)
        {
            bool isDead = false;

            // Se o Target for um UnityEngine.Object destru�do
            if (cb.Target is UnityEngine.Object unityObj)
            {
                if (unityObj == null)
                {
                    isDead = true;
                }
            }

            if (isDead)
            {
                // Remove do dicion�rio o callback morto
                subscribers[type] = (Action<T>)subscribers[type] - (Action<T>)cb;
                continue;
            }

            validCallbacks.Add((Action<T>)cb);
        }

        // Invoca apenas os callbacks ainda v�lidos
        foreach (var cb in validCallbacks)
        {
            try
            {
                cb.Invoke(message);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[EventBus] Exception while invoking {type.Name}: {ex}");
            }
        }
    }

    // -------------------------
    // limpar tudo (ex: trocar de cena)
    // -------------------------
    public static void ClearAll()
    {
        subscribers.Clear();
    }
}