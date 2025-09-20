using UnityEngine;

#region Core - Interfaces
/// <summary>
/// Interface base para todos os estados (macro e subestados).
/// </summary>
public interface IState
{
    /// <summary>Executado ao entrar no estado.</summary>
    void Enter();

    /// <summary>Executado a cada Update do jogo.</summary>
    void Update();

    /// <summary>Executado ao sair do estado.</summary>
    void Exit();
}
#endregion
