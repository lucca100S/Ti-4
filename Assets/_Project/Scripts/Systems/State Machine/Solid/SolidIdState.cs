using Systems.Input;
using UnityEngine;

#region Substates - Solid - Idle
/// <summary>Idle enquanto em modo Sólido.</summary>
public class SolidIdleState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;
    public SolidIdleState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter() 
    {
        Debug.Log("[SolidIdle] Enter");
        player.GetComponent<Animator>().ResetTrigger("Jump");
        player.GetComponent<Animator>().ResetTrigger("MeetGround");
        player.GetComponent<Animator>().ResetTrigger("Falling");
        player.GetComponent<Animator>().ResetTrigger("Walk");
        if (!player.IsGrounded)
        {
            player.GetComponent<Animator>().SetTrigger("MeetGround");
        }
    }
        

    public void Update()
    {

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
