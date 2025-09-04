using UnityEngine;

namespace Physics
{
    public class Gravity : ForceConstant
    {
        private float _gravityFactor = 1f;

        #region Properties

        public float GravityFactor { get => _gravityFactor; set => _gravityFactor = value; }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _force = 9.81f;
            _direction = Vector3.down;
            _timeInterval = 0;
        }
    }
}