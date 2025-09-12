using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>Contrato para objetos interativos.</summary>
public interface IInteractable
{
    Func<bool> IsInteractable { get; }
    void Interaction();
}

/// <summary>Contrato para filtros de detecção de colisão.</summary>
public interface ICollisionFilterDetection
{
    LayerMask CollisionMask { get; set; }
    HashSet<string> CollisionTags { get; set; }
}

/// <summary>Contrato para eventos de colisão via collider.</summary>
public interface IColliderEnterCollision : ICollisionFilterDetection
{
    void OnTriggerEnter(Collider other);
}

public interface IColliderStayCollision : ICollisionFilterDetection
{
    void OnTriggerStay(Collider other);
}

public interface IColliderExitCollision : ICollisionFilterDetection
{
    void OnTriggerExit(Collider other);
}

/// <summary>Agregador para todos os eventos de trigger.</summary>
public interface IColliderAllCollisionModes :
    IColliderEnterCollision, IColliderStayCollision, IColliderExitCollision
{ }

/// <summary>Contrato para detecção em esfera ao redor do objeto.</summary>
public interface ISurroundingsSphereDetection : ICollisionFilterDetection
{
    float Radius { get; set; }
    Vector3 SphereOrigin { get; set; }
    bool SurroundingsSphereDetection();
    void OnDrawGizmos();
}

public abstract class Obstacles : MonoBehaviour, IColliderEnterCollision
{
    public LayerMask CollisionMask { get; set; }
    public HashSet<string> CollisionTags { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Chamada indevida do método OnTriggerEnter(Collider other) de public abstract class Obstacles : MonoBehaviour, IColliderEnterCollision. Faça a implementação na classe concreta");
    }
}

public abstract class OptionalInteractableObjects : MonoBehaviour, IInteractable, ISurroundingsSphereDetection
{
    public Func<bool> IsInteractable { get => SurroundingsSphereDetection; }
    public float Radius { get; set; }
    public LayerMask CollisionMask { get; set; }
    public HashSet<string> CollisionTags { get; set; }
    public Vector3 SphereOrigin { get; set; }
    public virtual void Interaction()
    {
        if (IsInteractable()) { }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(SphereOrigin, Radius);
    }

    public bool SurroundingsSphereDetection()
    {
        var FoundColliders = UnityEngine.Physics.OverlapSphere(SphereOrigin, Radius, CollisionMask)
            .Where(c => CollisionTags.Contains(c.tag))
            .ToList();
        if (FoundColliders.Count > 0)
        {
            foreach (var col in FoundColliders)
            {
                Debug.Log($"Collider válido: {col.name}, {col.gameObject.tag}");
            }
            FoundColliders.Clear();
            return true;
        }
        FoundColliders.Clear();
        return false;
    }
}
public class CheckPoint : OptionalInteractableObjects
{

    /*Implementação da lógica base da interação, será delegado ao player para a ativação
     *  Execução da animação e efeitos viusais/sonoros
     *  Alocação do checkpoint como últimno checkpoint cadastrado pelo player e como checkpoint já cadastrado pelo player
    */
    public override void Interaction()
    {
        if (IsInteractable()) { }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(SphereOrigin, Radius);
    }
}
public class Collectables : OptionalInteractableObjects
{
    /*Implementação da lógica base da interação, será delegado ao player para a ativação
    *  Execução da animação e efeitos viusais/sonoros
    *  Alocação do coletável como coletado no Observer e no posterior SaveManager
    */
    public override void Interaction()
    {
        if (IsInteractable())
        {
            CollectableObservable.Instance.NotifyListeners(this);
        }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(SphereOrigin, Radius);
    }
}



/// <summary>
/// Contrato para qualquer objeto que observe eventos do tipo T.
/// </summary>
public interface IObserver<T>
{
    void OnCollectable(T item);
    void SetCollectedCollectables(List<T> itens);
}

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

/// <summary>
/// Contrato simples para singletons.
/// </summary>
public interface ISingleton<T>
{
    static T Instance { get; }
    void Awake();
}

/// <summary>
/// Classe observável para coletáveis, usando Singleton.
/// </summary>
public class CollectableObservable : MonoBehaviour, ISingleton<CollectableObservable>, IObservable<Collectables>
{
    public static CollectableObservable Instance { get; private set; }

    public List<Action<Collectables>> Observers { get; set; } = new List<Action<Collectables>>();

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SubscribeListeners(IObserver<Collectables> observer)
    {
        // Cria um delegate que encapsula a chamada para OnCollectable()
        Action<Collectables> callback = observer.OnCollectable;
        if (!Observers.Contains(callback))
            Observers.Add(callback);
    }

    public void UnsubscribeListeners(IObserver<Collectables> observer)
    {
        // Remove a função do observer da lista
        Action<Collectables> callback = observer.OnCollectable;
        if (Observers.Contains(callback))
            Observers.Remove(callback);
    }

    public void NotifyListeners(Collectables collectable)
    {
        foreach (var observer in Observers)
        {
            observer?.Invoke(collectable);
        }
    }
}

/// <summary>
/// Classe base para qualquer observer de coletáveis.
/// </summary>
public abstract class CollectableObserver : MonoBehaviour, IObserver<Collectables>
{
    protected void OnEnable()
    {
        CollectableObservable.Instance?.SubscribeListeners(this);
    }

    protected void OnDisable()
    {
        CollectableObservable.Instance?.UnsubscribeListeners(this);
    }

    public abstract void OnCollectable(Collectables item);

    public abstract void SetCollectedCollectables(List<Collectables> itens);
}

public class ObserverTest : CollectableObserver
{
    [SerializeField]
    TextMeshPro TextMeshPro { get; set; }
    int _quantity;

    private void Awake()
    {
        _quantity = 0;
        TextMeshPro.text = _quantity.ToString();
    }
    public override void OnCollectable(Collectables item)
    {
        _quantity++;
        TextMeshPro.text = _quantity.ToString();
    }

    public override void SetCollectedCollectables(List<Collectables> itens)
    {
        throw new NotImplementedException();
    }
}

public class ObserverSecondTest : CollectableObserver
{
    [SerializeField]
    TextMeshPro TextMeshPro { get; set; }
    int _quantity;

    private void Awake()
    {
        _quantity = 10;
        TextMeshPro.text = _quantity.ToString();
    }
    public override void OnCollectable(Collectables item)
    {
        _quantity -= 1;
        TextMeshPro.text = _quantity.ToString();
    }

    public override void SetCollectedCollectables(List<Collectables> itens)
    {
        throw new NotImplementedException();
    }
}