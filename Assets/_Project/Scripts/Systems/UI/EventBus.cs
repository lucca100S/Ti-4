using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// =====================================
// EVENT BUS GLOBAL
// =====================================
public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> subscribers = new();

    public static void Subscribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (subscribers.TryGetValue(type, out var existing))
            subscribers[type] = (Action<T>)existing + callback;
        else
            subscribers[type] = callback;
    }

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

    public static void Publish<T>(T message)
    {
        var type = typeof(T);
        if (subscribers.TryGetValue(type, out var callback))
            ((Action<T>)callback)?.Invoke(message);
    }
}

// =====================================
// EVENTOS DE UI
// =====================================
public struct SceneChangeEvent
{
    public string SceneName;
}

public struct PanelToggleEvent
{
    public string PanelName;
    public bool Active;
}

public struct ComponentTriggeredEvent
{
    public string ComponentName;
}

public struct EndApplicationEvent { }

public struct MasterVolumeChangeEvent
{
    public float NewVolume;
}

public struct MusicVolumeChangeEvent
{
    public float NewVolume;
}

public struct SFXVolumeChangeEvent
{
    public float NewVolume;
}

