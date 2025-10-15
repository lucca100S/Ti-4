using Systems.Input;
using UnityEngine;

#region Core - StateMachine
/// <summary>
/// Máquina de estados simples que gerencia transições e chamadas Update/Enter/Exit.
/// </summary>
public class StateMachine
{
    private float _timeInState;

    /// <summary>Estado atual da máquina.</summary>
    public IState CurrentState { get; private set; }
    public float TimeInState => _timeInState;

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
        _timeInState = 0;
        CurrentState.Enter();
    }

    /// <summary>Chama Update no estado atual.</summary>
    public void Update()
    {
        CurrentState?.Update();
        _timeInState += Time.deltaTime;
    }
    public void JumpInput(InputInfo input)
    {
        CurrentState?.OnJumpInput(input);
    }
}
#endregion
