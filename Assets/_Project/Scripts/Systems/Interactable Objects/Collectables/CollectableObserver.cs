using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Observer que registra coletáveis e os listeners que precisam ser notificados se coletáveis foram coletados ou não
/// </summary>
public class CollectableObserver : MonoBehaviour
{
    public static CollectableObserver Instance { get; private set; }

    private readonly List<Collectable> _collectableCache = new List<Collectable>();
    private readonly List<ICollectableListener> _listeners = new List<ICollectableListener>();

    public IReadOnlyList<Collectable> Collectables => _collectableCache;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Inscreve no evento global de coletáveis
        Collectable.OnCollected += AddCollectable;
    }

    void OnDestroy()
    {
        Collectable.OnCollected -= AddCollectable;
    }

    public void RegisterListener(ICollectableListener listener)
    {
        if (!_listeners.Contains(listener))
            _listeners.Add(listener);
    }

    public void UnregisterListener(ICollectableListener listener)
    {
        _listeners.Remove(listener);
    }

    private void AddCollectable(Collectable collectable)
    {
        if (!_collectableCache.Contains(collectable))
            _collectableCache.Add(collectable);

        NotifyListeners();
    }

    private void NotifyListeners()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            if (_listeners[i] == null)
            {
                _listeners.RemoveAt(i);
                continue;
            }

            _listeners[i].OnCollectableUpdated(_collectableCache);
        }
    }
}
