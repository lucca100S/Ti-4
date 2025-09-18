/// <summary>
/// Agrega todos os eventos de trigger em uma �nica interface.
/// �til para classes que precisam tratar entrada, perman�ncia e sa�da.
/// </summary>
public interface IColliderAllCollisionModes :
    IColliderEnterCollision, IColliderStayCollision, IColliderExitCollision
{ }
