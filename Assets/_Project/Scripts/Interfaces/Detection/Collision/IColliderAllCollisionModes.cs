/// <summary>
/// Agrega todos os eventos de trigger em uma única interface.
/// Útil para classes que precisam tratar entrada, permanência e saída.
/// </summary>
public interface IColliderAllCollisionModes :
    IColliderEnterCollision, IColliderStayCollision, IColliderExitCollision
{ }
