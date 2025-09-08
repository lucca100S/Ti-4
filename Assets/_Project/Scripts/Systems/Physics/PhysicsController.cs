using Lugu.Utils.Debug;
using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PhysicsController : MonoBehaviour
    {
        private Rigidbody _rigidBody;
        private Dictionary<string, PhysicsForce> _forces = new Dictionary<string, PhysicsForce>();

        private Vector3 _accumulatedForces = Vector3.zero;

        #region Properties

        public Rigidbody RigidBody { get => _rigidBody; }
        public Dictionary<string, PhysicsForce> Forces { get => _forces; }

        #endregion

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            UpdateForces();
            ApplyForces();
            CheckCollisions();
        }

        private void UpdateForces()
        {
            _accumulatedForces = Vector3.zero;

            foreach (PhysicsForce force in _forces.Values)
            {
                force.Update(Time.fixedDeltaTime);
                _accumulatedForces += force.ForceCurrent;
            }
        }
        private void ApplyForces()
        {
            _rigidBody.linearVelocity = _accumulatedForces;
            DebugUtil.Log($"Accumulated Forces on {gameObject.name}: {_accumulatedForces}", "Physics");
        }
        private void CheckCollisions()
        {
            foreach (PhysicsForce force in _forces.Values)
            {
                Vector3 position = transform.position;
                if(force.TargetPoint != null) position = force.TargetPoint.localPosition;

                UnityEngine.Physics.Raycast(position, force.Direction, out RaycastHit hit, 0.1f);
                if (hit.collider != null)
                {
                    force.OnHit?.Invoke(hit);
                }
                else if (force.IsHitting)
                {
                    force.OnExitHit?.Invoke();
                }
            }
        }

        public void AddForce(PhysicsForce force)
        {
            if (!_forces.ContainsKey(force.Name))
            {
                _forces.Add(force.Name, force);
            }
            else
            {
                _forces[force.Name] = force;
            }
        }
        public void RemoveForce(string id)
        {
            if (_forces.ContainsKey(id))
            {
                _forces.Remove(id);
            }
        }
        public void RemoveForce(PhysicsForce force)
        {
            RemoveForce(force.Name);
        }
        public PhysicsForce GetForce(string id)
        {
            if (_forces.ContainsKey(id))
            {
                return _forces[id];
            }
            return null;
        }
    }
}
