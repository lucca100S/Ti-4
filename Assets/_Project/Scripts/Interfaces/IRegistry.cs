using System;
using System.Collections.Generic;

/// <summary>
/// Generic interface for a registry of objects based on enumerated keys.
/// Designed for use in Editor-Driven systems (such as audio, VFX, etc.).
/// </summary>
/// <typeparam name="T">Type of object to be registered (e.g., audio ScriptableObject).</typeparam>
/// <typeparam name="U">Enum used as the identifying key.</typeparam>
public interface IRegistry<T, U> where U : Enum
{
    // ----------------------
    // Registration
    // ----------------------
    void Register(U id, T obj);
    bool Unregister(U id);
    bool Unregister(T obj);

    // ----------------------
    // Access
    // ----------------------
    T Get(U id);
    bool TryGet(U id, out T obj);
    U GetId(T obj);
    bool Contains(U id);
    bool Contains(T obj);
    IEnumerable<KeyValuePair<U, T>> GetAll();

    // ----------------------
    // Maintenance
    // ----------------------
    void Clear();
    bool ValidateRegistry();

    // ----------------------
    // Persistence
    // ----------------------
    void SaveState();
    void LoadState();

    // ----------------------
    // Debug
    // ----------------------
    void ShowAllObjects();
    void ShowAllKeys();
}
