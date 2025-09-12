/// <summary>
/// Contrato simples para singletons.
/// </summary>
public interface ISingleton<T>
{
    static T Instance { get; }
    void Awake();
}
