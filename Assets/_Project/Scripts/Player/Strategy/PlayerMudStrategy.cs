using UnityEngine;
using Player.Movement;

namespace Player.Strategy
{
    public class PlayerMudStrategy : PlayerStrategyScriptable
    {
        private PlayerStrategyHandler.Strategy _strategy = PlayerStrategyHandler.Strategy.Mud;
        private PlayerStrategyHandler.Strategy _nextStrategy = PlayerStrategyHandler.Strategy.Solid;

        [Header("MudStats")]
        [SerializeField] private float _airTurnSpeed = 1;

        public override PlayerStrategyHandler.Strategy Strategy { get => _strategy; protected set => _strategy = value; }
        public override PlayerStrategyHandler.Strategy NextStrategy { get => _nextStrategy; protected set => _nextStrategy = value; }


        public override void Jump(PlayerMovement player)
        {
            Vector3 force = player.Direction * GetJumpSpeed(player);

            force.y = _jumpForce;
            player.Force = force;
        }

        public override void Move(PlayerMovement player)
        {
            Vector3 movement = Vector3.one;

            switch(player.CurrentState)
            {
                case PlayerController.State.Air:
                    movement = player.Direction * GetJumpSpeed(player);
                    break;
                case PlayerController.State.Wall:
                    movement = player.Direction * GetClimbSpeed(player);
                    break;
                case PlayerController.State.Ground:
                    movement = player.Direction * GetWalkSpeed(player);
                    break;
            }
            

            movement.y = 0;
            player.Force = new Vector3(movement.x, player.Force.y, movement.z);

            player.CharacterController.Move(player.Force * Time.deltaTime);
        }

        public override void Transform(PlayerMovement player)
        {
            Physics.Raycast(player.transform.position, Vector3.up, out RaycastHit hitInfo, player.GetStrategy(_nextStrategy).Height, LayerMask.GetMask("Map"));
            if (hitInfo.collider == null)
            {
                player.ChangeStrategy(_nextStrategy);
            }
        }

        public override void GetDirection(PlayerMovement player)
        {

            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = Camera.main.transform.right;
            right.y = 0;
            right.Normalize();

            switch (player.CurrentState)
            {
                case PlayerController.State.Air:
                    if(player.Direction != Vector3.zero)
                    player.Direction = Vector3.Lerp(player.Direction, forward * player.Input.z + right * player.Input.x, _airTurnSpeed * Time.deltaTime).normalized;
                    break;
                case PlayerController.State.Wall:
                    break;
                case PlayerController.State.Ground:

                    player.Direction = forward * player.Input.z + right * player.Input.x;
                    break;
            }
        }
        public override void Rotate(PlayerMovement player)
        {
            if (player.Direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(player.Force, Vector3.up);
                toRotation = Quaternion.Euler(0, toRotation.eulerAngles.y, 0);

                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, toRotation, 720 * Time.deltaTime);
            }
        }

    }
}
