using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

namespace Physics
{
    [RequireComponent(typeof(IMovable))]
    public class ForceConstant : MonoBehaviour
    {
        protected IMovable _movable;
        [SerializeField] protected float _force = 1;
        [SerializeField] protected Vector3 _direction = Vector3.down;
        [SerializeField] protected float _timeInterval = 0;

        protected float _timeElapsed = 0;

        #region Properties

        public IMovable Movable{ get => _movable; private set => _movable = value; }

        public float Force { get => _force; set => _force = value; }
        public Vector3 Direction { get => _direction; set => _direction = value; }
        public float TimeInterval { get => _timeInterval; set => _timeInterval = Mathf.Max(value, 0); }
        public float TimeElapsed { get => _timeElapsed; private set => _timeElapsed = value; }

        #endregion

        protected virtual void Awake()
        {
            if (_movable == null) _movable = GetComponent<IMovable>();
        }

        protected virtual void Update()
        {
            if (_timeInterval > 0)
            {
                _timeElapsed += Time.deltaTime;
                if (_timeElapsed >= _timeInterval)
                {
                    ApplyForce();
                    _timeElapsed = 0;
                }
            }
            else
            {
                ApplyForce();
            }
        }

        protected virtual void ApplyForce()
        {
            _movable.AddForce(_direction * _force);
        }
    }
}
