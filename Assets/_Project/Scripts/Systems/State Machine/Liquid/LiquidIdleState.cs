using Systems.Input;
using UnityEngine;

#region Substates - Liquid - Idle
/// <summary>
/// Estado Idle no modo Líquido.
/// Não aplica movimento, serve como estado "neutro".
/// </summary>
public class LiquidIdleState : IState
{
    private readonly LiquidoState parent;
    private readonly PlayerStateMachine player;
    private readonly SurfaceDetection surface;

    private float _variationTimeMin = 10f;
    private float _variationTimeMax = 15f;

    private float _currentVariationTime;
    public StateType StateType => StateType.Idle;

    public LiquidIdleState(LiquidoState parent, PlayerStateMachine player, SurfaceDetection surface)
    {
        this.parent = parent;
        this.player = player;
        this.surface = surface;
    }

    public void Enter()
    {
        Debug.Log("[LiquidIdle] Enter");

        _currentVariationTime = Random.Range(_variationTimeMin, _variationTimeMax);
    }

    public void Update()
    {
        if (parent.TimeInState >= _currentVariationTime)
        {
            ActionsManager.Instance.OnStateAnimationChanged?.Invoke("LookAround", 0.2f);
            _currentVariationTime += Random.Range(_variationTimeMin, _variationTimeMax);
        }
    }

    public void Exit()
    {
        Debug.Log("[LiquidIdle] Exit");
    }

    public void OnJumpInput(InputInfo input)
    {

    }
}
#endregion
