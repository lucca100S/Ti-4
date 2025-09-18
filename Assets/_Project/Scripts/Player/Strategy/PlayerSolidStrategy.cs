using UnityEngine;
using Player.Movement;

namespace Player.Strategy
{
    //[CreateAssetMenu(fileName = "PlayerSolidStrategy", menuName = "ScriptableObjects/Strategy/Solid")]
    public class PlayerSolidStrategy : PlayerStrategyScriptable
    {
        private PlayerStrategyHandler.Strategy _strategy = PlayerStrategyHandler.Strategy.Solid;
        private PlayerStrategyHandler.Strategy _nextStrategy = PlayerStrategyHandler.Strategy.Mud;

        public override PlayerStrategyHandler.Strategy Strategy { get => _strategy; protected set => _strategy = value; }
        public override PlayerStrategyHandler.Strategy NextStrategy { get => _nextStrategy; protected set => _nextStrategy = value; }

        public override void Jump(PlayerMovement player)
        {
            player.Force = new Vector3(player.Force.x, _jumpForce, player.Force.z);
        }

        public override void Move(PlayerMovement player)
        {
            Vector3 movement = player.Direction * _speed;
            movement.y = 0;

            player.Force = new Vector3(movement.x, player.Force.y, movement.z);

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
            player.Direction = forward * player.Input.z + right * player.Input.x;
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
