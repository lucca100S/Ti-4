using UnityEngine;

namespace Physics
{
    public class Gravity : ForceConstant
    {
        [SerializeField] private float _gravityFactor = 1f;
        [SerializeField] private float _maxGravity = 50f;

        #region Properties

        public float GravityFactor { get => _gravityFactor; set => _gravityFactor = value; }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _force = new PhysicsForce("Gravity", Vector3.down, _maxGravity, 9.81f * _gravityFactor, 0, -1, false, true);
        }

        public void ChangeGravityFactor()
        {
            _force.ForceAcceleration = 9.81f * _gravityFactor;
        }

        private void OnCollisionStay(Collision collision)
        {
            _force.SetForce(Vector3.zero);
        }

    }
}