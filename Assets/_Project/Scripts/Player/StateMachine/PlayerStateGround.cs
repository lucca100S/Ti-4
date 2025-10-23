using UnityEngine;

namespace Player.StateMachine
{
    public class PlayerStateGround : PlayerState
    {
        public override void Enter(PlayerController player)
        {

        }

        public override void Exit(PlayerController player)
        {

        }

        public override void StateUpdate(PlayerController player)
        {
            //player.PlayerMovement.Grounded(true);
        }
    }
}
