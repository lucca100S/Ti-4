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
        player.GetComponent<Animator>().SetBool("Liquid", false);
        player.GetComponent<Animator>().SetBool("OnIdle", true);
        
    }
        

    public void Update()
    {
        // nada além de manter posição — mas podemos tocar animação aqui
        // Ex: parent.Player.Animator.Play("Idle");
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
