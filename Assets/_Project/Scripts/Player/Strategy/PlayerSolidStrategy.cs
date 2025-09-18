using UnityEngine;
using Player.Movement;

namespace Player.Strategy
{
    //[CreateAssetMenu(fileName = "PlayerSolidStrategy", menuName = "ScriptableObjects/Strategy/Solid")]
    public class PlayerSolidStrategy : PlayerStrategyScriptable
    {
        private PlayerStrategyHandler.Strategy _strategy = PlayerStrategyHandler.Strategy.Solid;
        private PlayerStrategyHandler.Strategy _nextStrategy = PlayerStrategyHandler.Strategy.Mud;

        public override PlayerStrategyHandler.Strategy strategy { get => _strategy; protected set => _strategy = value; }
        public override PlayerStrategyHandler.Strategy nextStrategy { get => _nextStrategy; protected set => _nextStrategy = value; }

        public override void Jump(PlayerMovement player)
        {
            player.force = new Vector3(player.force.x, _jumpForce, player.force.z);
        }

        public override void Move(PlayerMovement player)
        {
            Vector3 movement = player.direction * _speed;
            movement.y = _gravity;

            player.HandleGravity();

            player.force = new Vector3(movement.x, player.force.y, movement.z);

            player.characterController.Move(player.force * Time.deltaTime);
        }

        public override void Transform(PlayerMovement player)
        {
            player.ChangeStrategy(_nextStrategy);
        }

        public override void GetDirection(PlayerMovement player)
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = Camera.main.transform.right;
            right.y = 0;
            right.Normalize();
            player.direction = forward * player.input.z + right * player.input.x;
        }
        public override void Rotate(PlayerMovement player)
        {
            if (player.direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(player.force, Vector3.up);
                toRotation = Quaternion.Euler(0, toRotation.eulerAngles.y, 0);

                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, toRotation, 720 * Time.deltaTime);
            }
        }
    }
}
