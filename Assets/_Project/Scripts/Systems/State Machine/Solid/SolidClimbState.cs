using UnityEngine;

#region Substates - Solid - Climb
/// <summary>Escalada em Sólido.</summary>
public class SolidClimbState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    public SolidClimbState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter() => Debug.Log("[SolidClimb] Enter");

    public void Update()
    {
        // Subir ao longo da parede enquanto W pressionado
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 up = Vector3.up * (player != null ? player.SolidSpeed * 0.6f : 3f);
            player?.SetMovement(up);
        }
        else
        {
            // Se soltar W, retornar ao macro (macro decide transição)
        }
    }

    public void Exit() => Debug.Log("[SolidClimb] Exit");
}
#endregion
