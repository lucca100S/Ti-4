using UnityEngine;
using Player.Movement;

namespace Player.Strategy
{
    //[CreateAssetMenu(fileName = "PlayerMudStrategy", menuName = "ScriptableObjects/Strategy/Mud")]
    public class PlayerMudStrategy : PlayerStrategyScriptable
    {
        private PlayerStrategyHandler.Strategy _strategy = PlayerStrategyHandler.Strategy.Mud;
        private PlayerStrategyHandler.Strategy _nextStrategy = PlayerStrategyHandler.Strategy.Solid;

        [Header("MudStats")]
        [SerializeField] private float _launchSpeed = 1;
        [SerializeField] private float _airTurnSpeed = 1;

        public override PlayerStrategyHandler.Strategy strategy { get => _strategy; protected set => _strategy = value; }
        public override PlayerStrategyHandler.Strategy nextStrategy { get => _nextStrategy; protected set => _nextStrategy = value; }


        public override void Jump(PlayerMovement player)
        {
            Vector3 force = player.direction * _launchSpeed;

            force.y = _jumpForce;
            player.force = force;
        }

        public override void Move(PlayerMovement player)
        {
            Vector3 movement = Vector3.one;

            switch (player.currentState)
            {
                case PlayerMovement.State.Jumping:
                case PlayerMovement.State.Falling:
                    movement = player.direction * _launchSpeed;
                    break;
                case PlayerMovement.State.Climbing:
                    break;
                default:
                    movement = player.direction * _speed;
                    break;
            }

            movement.y = _gravity;
            player.force = new Vector3(movement.x, player.force.y, movement.z);

            player.HandleGravity();
            player.characterController.Move(player.force * Time.deltaTime);
        }

        public override void Transform(PlayerMovement player)
        {
            Physics.Raycast(player.transform.position, Vector3.up, out RaycastHit hitInfo, player.GetStrategy(_nextStrategy).height);
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

            switch (player.currentState)
            {
                case PlayerMovement.State.Jumping:
                case PlayerMovement.State.Falling:
                    player.direction = Vector3.Lerp(player.direction, forward * player.input.z + right * player.input.x, _airTurnSpeed * Time.deltaTime);
                    break;
                case PlayerMovement.State.Climbing:
                    break;
                default:

                    player.direction = forward * player.input.z + right * player.input.x;
                    break;
            }
        }
        public override void Rotate(PlayerMovement player)
        {
            if (player.direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(player.force, Vector3.up);
                toRotation = Quaternion.Euler(toRotation.eulerAngles.x, toRotation.eulerAngles.y, 0);

                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, toRotation, 720 * Time.deltaTime);
            }
            else
            {
                Quaternion toRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);

                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, toRotation, 720 * Time.deltaTime);
            }
        }

    }
}
