using Lugu.Utils.Debug;
using Physics;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(PhysicsController), typeof(Gravity))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform _alignTarget;
        private PhysicsController _physicsController;

        [Header("Stats")]
        [SerializeField] private PhysicsForce _forceMovement;
        [SerializeField] private PhysicsForce _forceJump;
        private Gravity _gravity;

        private Vector3 _direction;
        private bool _canJump;

        #region Properties

        public Transform AlignTarget { get => _alignTarget; set => _alignTarget = value; }
        public bool CanJump { get => _canJump; set => _canJump = value; }

        #endregion

        private void Awake()
        {
            _physicsController = GetComponent<PhysicsController>();
            _gravity = GetComponent<Gravity>();
        }
        private void OnEnable()
        {
            _physicsController.AddForce(_forceMovement);
            _physicsController.AddForce(_forceJump);

            _forceJump.OnForceDeactivate += () => _gravity.Force.Activate();
            _forceJump.OnForceActivate += () => _gravity.Force.Deactivate();

            _gravity.Force.OnHit += (hit) => _canJump = true;
            _gravity.Force.OnExitHit += () => _canJump = false;
        }
        private void OnDisable()
        {
            _physicsController.RemoveForce(_forceMovement);
            _physicsController.RemoveForce(_forceJump);
        }
        private void Update()
        {
            Vector3 direction = _direction;
            if (_alignTarget != null)
            {
                AlignWithTarget(ref direction);
            }
            _forceMovement.SetDirection(direction);
        }

        public void ChangeDirection(Vector3 direction)
        {
            _direction = direction.normalized;
        }
        public void Jump(bool isPressed)
        {
            if (isPressed && _canJump)
            {
                _forceJump.Activate();
            }
            else
            {
                _forceJump.Deactivate();
            }
        }
        private void AlignWithTarget(ref Vector3 direction)
        {
            Vector3 targetForward = _alignTarget.forward;
            Vector3 targetRight = _alignTarget.right;
            Vector3 targetUp = _alignTarget.up;
            
            //Change to plane projection later
            direction = direction.x * targetRight + direction.y * targetUp + direction.z * targetForward;
            direction.y = 0; // Keep movement on the horizontal plane
            direction.Normalize();
        }
    }
}
