using UnityEngine;
using Player.Movement;

namespace Player.Strategy
{
    public class PlayerSolidStrategy : PlayerStrategyScriptable
    {
        private PlayerStrategyHandler.Strategy _strategy = PlayerStrategyHandler.Strategy.Solid;
        private PlayerStrategyHandler.Strategy _nextStrategy = PlayerStrategyHandler.Strategy.Mud;

        public override PlayerStrategyHandler.Strategy Strategy { get => _strategy; protected set => _strategy = value; }
        public override PlayerStrategyHandler.Strategy NextStrategy { get => _nextStrategy; protected set => _nextStrategy = value; }

        public override void Jump(PlayerMovement player)
        {
            switch (player.CurrentState)
            { 
                case PlayerController.State.Wall:
                    Vector3 forceDirection = -player.SurfaceDetection.CurrentSurface.Value.hit.normal;
                    player.Force = forceDirection * _wallJumpForceDistance + player.transform.up * _wallJumpForceHeight;
                    break;
                case PlayerController.State.Ground:
                case PlayerController.State.Air:
                    player.Force = new Vector3(player.Force.x, _jumpForce, player.Force.z);
                    break;
            }
            
        }
        public override void Move(PlayerMovement player)
        {
            Vector3 movement = player.Direction * GetWalkSpeed(player);
            Vector3 force = Vector3.zero;

            switch (player.CurrentState)
            {
                case PlayerController.State.Air:
                    force = MoveAir(player, movement);
                    break;
                case PlayerController.State.Wall:
                    movement = player.Direction * GetClimbSpeed(player);
                    

                    switch (player.CurrentMaterial)
                    { 
                        case SurfaceMaterial.Earth:
                            force = new Vector3(movement.x, player.Force.y, movement.z);
                            break;
                        case SurfaceMaterial.Stone:
                            force = new Vector3(0, 0, 0);
                            break;
                        case SurfaceMaterial.Vines:
                            force = new Vector3(movement.x, movement.y, movement.z);
                            break;
                        default:
                            force = MoveAir(player, movement);
                            break;
                    }
                    


                    break;
                case PlayerController.State.Ground:
                    movement = player.Direction * GetWalkSpeed(player);
                    movement.y = 0;

                    force = new Vector3(movement.x, player.Force.y, movement.z);
                    break;
            }

            player.Force = force;
        }
        public override void Transform(PlayerMovement player)
        {
            player.ChangeStrategy(_nextStrategy);
        }
        public override void GetDirection(PlayerMovement player)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            switch (player.CurrentState)
            {
                case PlayerController.State.Wall:
                    forward = player.transform.up;
                    right = player.transform.right;

                    break;
                default:
                    forward.y = 0;
                    forward.Normalize();


                    right.y = 0;
                    right.Normalize();
                    break;
            }
            player.Direction = (forward * player.Input.z + right * player.Input.x).normalized;
        }   
        public override void Rotate(PlayerMovement player)
        {
            switch(player.CurrentState)
            {
                case PlayerController.State.Wall:

                        Vector3 lookPos = -player.SurfaceDetection.CurrentSurface.Value.hit.normal;
                        player.transform.rotation = Quaternion.LookRotation(lookPos, Vector3.up);
                    
                    break;
                default:
                    if (player.Direction != Vector3.zero)
                    {
                        Quaternion toRotation = Quaternion.LookRotation(player.Force, Vector3.up);
                        toRotation = Quaternion.Euler(0, toRotation.eulerAngles.y, 0);

                        player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, toRotation, 720 * Time.deltaTime);
                    }
                    break;
            }
            
        }

        private Vector3 MoveAir(PlayerMovement player, Vector3 movement)
        {
            movement = player.Direction * GetJumpSpeed(player);
            movement.y = 0;

            return new Vector3(movement.x, player.Force.y, movement.z);
        }
    }
}
