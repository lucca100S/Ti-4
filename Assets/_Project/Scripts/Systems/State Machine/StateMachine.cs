using UnityEngine;

#region Core - StateMachine
/// <summary>
/// Máquina de estados simples que gerencia transições e chamadas Update/Enter/Exit.
/// </summary>
public class StateMachine
{
    /// <summary>Estado atual da máquina.</summary>
    public IState CurrentState { get; private set; }

    /// <summary>Muda para um novo estado (chama Exit no atual e Enter no novo).</summary>
    public void ChangeState(IState newState)
    {
        if (newState == null)
        {
            Debug.LogWarning("[StateMachine] Tentativa de trocar para null.");
            return;
        }

        if (CurrentState == newState)
            return;

        Debug.Log($"[StateMachine] Mudando estado: {CurrentState?.GetType().Name ?? "NULL"} -> {newState.GetType().Name}");
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    /// <summary>Chama Update no estado atual.</summary>
    public void Update()
    {
        CurrentState?.Update();
    }
}
#endregion
