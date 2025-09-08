using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

namespace Physics
{
    [RequireComponent(typeof(PhysicsController))]
    public class ForceConstant : MonoBehaviour
    {
        private PhysicsController _physicsController;
        [SerializeField] protected PhysicsForce _force;

        #region Properties

        public PhysicsForce Force { get => _force;}

        #endregion

        protected virtual void Awake()
        {
            _physicsController = GetComponent<PhysicsController>();
        }

        private void OnEnable()
        {
            if (_force != null)
            {
                _physicsController.AddForce(_force);
            }
            else
            {
                Debug.LogWarning($"No force assigned to {gameObject.name} ForceConstant component.");
            }
        }

        private void OnDisable()
        {
            if (_force != null)
            {
                _physicsController.RemoveForce(_force);
            }
        }
    }
}
