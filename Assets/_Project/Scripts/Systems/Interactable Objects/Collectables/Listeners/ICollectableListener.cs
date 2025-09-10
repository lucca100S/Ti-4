using System.Collections.Generic;
/// <summary>
/// Interface que define o contrato de todo e qualquer listener
/// </summary>
public interface ICollectableListener
{
    void OnCollectableUpdated(IReadOnlyList<Collectable> collectables);
}
