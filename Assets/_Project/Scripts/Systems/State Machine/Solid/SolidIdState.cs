using UnityEngine;

#region Substates - Solid - Idle
/// <summary>Idle enquanto em modo Sólido.</summary>
public class SolidIdleState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;

    public SolidIdleState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
    }

    public void Enter() => Debug.Log("[SolidIdle] Enter");

    public void Update()
    {
        // nada além de manter posição — mas podemos tocar animação aqui
        // Ex: parent.Player.Animator.Play("Idle");
    }

    public void Exit() => Debug.Log("[SolidIdle] Exit");
}
#endregion
