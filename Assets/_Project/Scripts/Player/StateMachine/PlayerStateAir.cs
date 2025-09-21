using UnityEngine;

namespace Player.StateMachine
{
    public class PlayerStateAir : PlayerState
    {
        public override void Enter(PlayerController player)
        {
            Vector3 force = player.PlayerMovement.Force;
            if (force.y < 0)
            {
                force.y = 0;
                player.PlayerMovement.Force = force;
            }
        }

        public override void Exit(PlayerController player)
        {
            player.PlayerMovement.IsWallJumping = false;
        }

        public override void StateUpdate(PlayerController player)
        {
            player.PlayerMovement.Grounded(false);
        }
    }
}
