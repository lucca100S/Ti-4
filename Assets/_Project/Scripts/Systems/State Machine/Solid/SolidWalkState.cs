using Systems.Input;
using UnityEngine;

#region Substates - Solid - Walk
/// <summary>Andar no modo S�lido.</summary>
public class SolidWalkState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    public SolidWalkState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = parent is SolidoState s ? (SolidoStateAccessor.GetPlayer(s)) : null;
    }

    public void Enter() 
    {
        AudioPlayer.Play(AudioId.Walk, PlaybackForce.Normal);
        Debug.Log("[SolidWalk] Enter");
    }
    

    public void Update()
    {
        Vector3 move = player.DirectionInput * (player != null ? player.SolidSpeed : 6f);
        player?.SetMovement(move);
    }

    public void Exit() { 
        AudioPlayer.Stop(AudioId.Walk); 
        Debug.Log("[SolidWalk] Exit"); 
    }

    public void OnJumpInput(InputInfo input)
    {
    }
}
#endregion

// Helper accessor to break circular dependency (internal utility)
static class SolidoStateAccessor
{
    public static PlayerStateMachine GetPlayer(SolidoState s)
    {
        // usa reflex�o controlada (somente leitura) - mas como alternativa direta,
        // a solu��o ideal seria passar PlayerStateMachine diretamente para cada subestado.
        // Por�m para simplicidade e seguran�a aqui, vamos tentar obter via campo privado (n�o ideal).
        // Para evitar reflection complexities no snippet, subestados j� podem obter player atrav�s do parent reference
        // se voc� preferir, altere os construtores para receber PlayerStateMachine explicitamente.
        return UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }
}
