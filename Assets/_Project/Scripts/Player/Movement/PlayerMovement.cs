using Lugu.Utils.Debug;
using Physics;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(PhysicsController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform _alignTarget;
        private PhysicsController _physicsController;

        [Header("Stats")]
        [SerializeField] private PhysicsForce _force;

        private Vector3 _direction;

        #region Properties

        public Transform AlignTarget { get => _alignTarget; set => _alignTarget = value; }

        #endregion

        private void Awake()
        {
            _physicsController = GetComponent<PhysicsController>();
        }
        private void OnEnable()
        {
            _physicsController.AddForce(_force);
        }

        private void OnDisable()
        {
            _physicsController.RemoveForce(_force);
        }

        private void Update()
        {
            Vector3 direction = _direction;
            if (_alignTarget != null)
            {
                AlignWithTarget(ref direction);
            }
            _force.SetDirection(direction);
        }

        public void ChangeDirection(Vector3 direction)
        {
            _direction = direction.normalized;
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
