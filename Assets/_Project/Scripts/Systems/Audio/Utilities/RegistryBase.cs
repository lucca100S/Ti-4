using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract class for registries, fully generic for any object type and enum key.
/// Uses RegistryErrorCode for detailed error logging.
/// </summary>
/// <typeparam name="T">Type of object to be registered.</typeparam>
/// <typeparam name="U">Enum type used as the key.</typeparam>
public abstract class RegistryBase<T, U> : MonoBehaviour, IRegistry<T, U> where U : Enum
{
    // ----------------------
    // Internal storage
    // ----------------------
    protected Dictionary<U, T> registry = new Dictionary<U, T>();

    // ----------------------
    // Registration
    // ----------------------
    public virtual void Register(U id, T obj)
    {
        if (obj == null)
        {
            Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.NullObject, obj, id));
            return;
        }

        if (registry.ContainsKey(id))
        {
            Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.DuplicateKey, obj, id));
            return;
        }

        if (registry.ContainsValue(obj))
        {
            Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.DuplicateKey, obj, "Object already registered with another ID"));
            return;
        }

        registry[id] = obj;
        Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] Registered object {obj} with ID {id}");
    }

    public virtual bool Unregister(U id)
    {
        if (registry.TryGetValue(id, out T obj))
        {
            if (obj == null)
            {
                Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.NullObject, obj, id));
            }

            registry.Remove(id);
            Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] Unregistered object {obj} with ID {id}");
            return true;
        }

        Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.KeyNotFound, null, id));
        return false;
    }

    public virtual bool Unregister(T obj)
    {
        if (obj == null)
        {
            Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.NullObject, obj));
            return false;
        }

        U key = default;
        bool found = false;
        foreach (var pair in registry)
        {
            if (EqualityComparer<T>.Default.Equals(pair.Value, obj))
            {
                key = pair.Key;
                found = true;
                break;
            }
        }

        if (found)
        {
            registry.Remove(key);
            Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] Unregistered object {obj} with ID {key}");
            return true;
        }

        Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.ObjectNotFound, obj));
        return false;
    }

    // ----------------------
    // Access
    // ----------------------
    public virtual T Get(U id)
    {
        if (registry.TryGetValue(id, out T obj))
        {
            return obj;
        }

        Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.KeyNotFound, null, id));
        return default;
    }

    public virtual bool TryGet(U id, out T obj)
    {
        bool result = registry.TryGetValue(id, out obj);
        if (!result)
        {
            Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.KeyNotFound, null, id));
        }
        else if (obj == null)
        {
            Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.NullObject, obj, id));
        }

        return result;
    }

    public virtual U GetId(T obj)
    {
        if (obj == null)
        {
            Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.NullObject, obj));
            return default;
        }

        foreach (var pair in registry)
        {
            if (EqualityComparer<T>.Default.Equals(pair.Value, obj))
                return pair.Key;
        }

        Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.ObjectNotFound, obj));
        return default;
    }

    public virtual bool Contains(U id) => registry.ContainsKey(id);

    public virtual bool Contains(T obj) => registry.ContainsValue(obj);

    public virtual IEnumerable<KeyValuePair<U, T>> GetAll() => registry;

    // ----------------------
    // Maintenance
    // ----------------------
    public virtual void Clear()
    {
        registry.Clear();
        Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] Registry cleared");
    }

    public virtual bool ValidateRegistry()
    {
        bool valid = true;
        foreach (var pair in registry)
        {
            if (pair.Value == null)
            {
                Debug.LogWarning(RegistryErrorCodeExtensions.GetMessage(RegistryErrorCode.NullObject, pair.Value, pair.Key));
                valid = false;
            }
        }

        Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] Registry validation completed, valid: {valid}");
        return valid;
    }

    // ----------------------
    // Persistence
    // ----------------------
    public virtual void SaveState()
    {
        Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] SaveState called (Editor-Driven, usually no-op)");
    }

    public virtual void LoadState()
    {
        Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] LoadState called (Editor-Driven, usually no-op)");
    }

    // ----------------------
    // Debug
    // ----------------------
    public virtual void ShowAllObjects()
    {
        Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] Showing all objects:");
        foreach (var pair in registry)
            Debug.Log($"ID: {pair.Key}, Object: {pair.Value}");
    }

    public virtual void ShowAllKeys()
    {
        Debug.Log($"[RegistryBase<{typeof(T).Name}, {typeof(U).Name}>] Showing all keys:");
        foreach (var key in registry.Keys)
            Debug.Log($"ID: {key}");
    }
}
