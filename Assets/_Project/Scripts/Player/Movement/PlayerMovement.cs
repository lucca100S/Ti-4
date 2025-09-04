using Physics;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IMovable
    {

        private Rigidbody _rigidBody;

        #region Properties
        
        public Vector3 CurrentForce { get => _rigidBody.GetAccumulatedForce(); }
        public Vector3 CurrentVelocity { get => _rigidBody.linearVelocity;}

        #endregion

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void AddForce(Vector3 force)
        {
            _rigidBody.AddForce(force);
        }

        public void SetForce(Vector3 force)
        {
            
        }

        public void SetVelocity(Vector3 velocity)
        {
            
        }

        public void AddVelocity(Vector3 velocity)
        {
            
        }
    }
}
