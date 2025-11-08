using Player;
using Systems.Input;
using UnityEngine;

#region Substates - Solid - Idle
/// <summary>Idle enquanto em modo Sólido.</summary>
public class SolidIdleState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    private float _variationTimeMin = 10f;
    private float _variationTimeMax = 15f;

    private float _currentVariationTime;

    public StateType StateType => StateType.Idle;

    public SolidIdleState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter() 
    {
        Debug.Log("[SolidIdle] Enter");
        AudioPlayer.Stop(AudioId.SolidStep);
        if (!player.IsGrounded)
        {
           
        }

        _currentVariationTime = Random.Range(_variationTimeMin, _variationTimeMax);
    }
        

    public void Update()
    {
        if(parent.TimeInState >= _currentVariationTime)
        {
            ActionsManager.Instance.OnStateAnimationChanged?.Invoke("LookAround", 0.2f);
            _currentVariationTime += Random.Range(_variationTimeMin, _variationTimeMax);
        }
    }

    public void Exit() 
    {
        Debug.Log("[SolidIdle] Exit");
    }
    

    public void OnJumpInput(InputInfo input)
    {
        
    }
}
#endregion
