using Lugu.Utils.Debug;
using Physics;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IMovable
    {
        private const ForceMode FORCE_MODE = ForceMode.Acceleration;
        private Rigidbody _rigidBody;

        private Vector3 _currentDirection = Vector3.zero;
        private Vector3 _currentVelocity = Vector3.zero;
        [SerializeField] private Transform _alignTarget;

        [Header("Stats")]
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _acceleration = 5f;
        [SerializeField] private float _deceleration = 5f;

        private float _currentSpeed = 0f;

        #region Properties

        public Vector3 CurrentVelocity { get => _currentVelocity; }
        public Vector3 CurrentDirection { get => _currentDirection; }
        public Transform AlignTarget { get => _alignTarget; set => _alignTarget = value; }

        #endregion

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 direction = _currentDirection;
            if (_alignTarget != null)
            {
                AlignWithTarget(ref direction);
            }

            if (direction.magnitude > 0)
            {
                AddForce(_acceleration * direction.normalized);
            }
            else
            {
                AddForce(_deceleration * (-_currentVelocity.normalized));
            }

            _currentVelocity = _currentSpeed * direction;

            _rigidBody.linearVelocity = _currentVelocity;
        }
        public void AddForce(Vector3 force)
        {
            float forceMagnitude = force.magnitude * Time.deltaTime;

            if(forceMagnitude + Mathf.Abs(_currentSpeed) > _maxSpeed)
            {
                forceMagnitude = _maxSpeed - Mathf.Abs(_currentSpeed);
            }

            DebugUtil.Log($"Player add force: {forceMagnitude}", "PlayerMovement");
            _currentSpeed += forceMagnitude;
            
        }
        public void SetForce(Vector3 force)
        {
            DebugUtil.Log($"Player set force: {force}", "PlayerMovement");
            float forceMagnitude = force.magnitude;
            
            float speedDifference = forceMagnitude - _currentSpeed;

            AddForce(force.normalized * speedDifference);
        }
        public void ChangeDirection(Vector3 direction)
        {
            _currentDirection = direction.normalized;
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
