using System;
using UnityEngine;

namespace Physics
{
    [System.Serializable]
    public class PhysicsForce
    {
        [Header("Control")]
        [SerializeField] private string _name = "Force";
        [SerializeField] private float _duration = 1;
        private float _timeElapsed = 0;
        [SerializeField] private Transform _targetPoint = null;

        [SerializeField] private bool _resetOnActivate = false;
        [SerializeField] private bool _resetOnDeactivate = false;
        private bool _isActive = true;
        private bool _isHitting = false;
        [SerializeField] private Color _color = Color.white;

        [Header("Force")]
        [SerializeField] private float _forceMax = 1;
        private Vector3 _forceCurrent = Vector3.zero;
        [SerializeField] private float _forceAcceleration = 1;
        [SerializeField] private float _forceDeceleration = 1;

        [SerializeField] private Vector3 _direction = Vector3.forward;


        #region Properties

        public bool IsActive { get => _isActive; }
        public bool IsHitting { get => _isHitting; }
        public Vector3 Direction { get => _direction;}
        public float ForceMax { get => _forceMax; set => _forceMax = value; }
        public Vector3 ForceCurrent { get => _forceCurrent; }
        public float ForceAcceleration { get => _forceAcceleration; set => _forceAcceleration = value; }
        public float ForceDeceleration { get => _forceDeceleration; set => _forceDeceleration = value; }
        public float Duration { get => _duration; set => _duration = value; }
        public Transform TargetPoint { get => _targetPoint; set => _targetPoint = value; }
        public float TimeElapsed { get => _timeElapsed; }
        public string Name { get => _name; set => _name = value; }
        public bool ResetOnActivate { get => _resetOnActivate; set => _resetOnActivate = value; }
        public bool ResetOnDeactivate { get => _resetOnDeactivate; set => _resetOnDeactivate = value; }
        public Color Color { get => _color; set => _color = value; }


        #endregion

        #region Actions

        public Action OnForceEnd;
        public Action<RaycastHit> OnHit;
        public Action OnExitHit;
        public Action OnForceActivate;
        public Action OnForceDeactivate;

        #endregion

        public PhysicsForce(string name, Vector3 direction, float forceMax, float forceAcceleration, float forceDeceleration, float duration, bool resetOnActivate = false, bool resetOnDeactivate = false)
        {
            _name = name;
            _direction = direction;
            _forceMax = forceMax;
            _forceCurrent = Vector3.zero;
            _forceAcceleration = forceAcceleration;
            _forceDeceleration = forceDeceleration;
            _duration = duration;
            _timeElapsed = 0f;
            _isActive = true;
            _resetOnActivate = resetOnActivate;
            _resetOnDeactivate = resetOnDeactivate;

            OnHit += (hit) => _isHitting = true;
            OnExitHit += () => _isHitting = false;
        }

        public void Activate(bool resetCurrentForce = false)
        {
            _isActive = true;
            _timeElapsed = 0f;
            if(resetCurrentForce) SetForce(Vector3.zero);

            OnForceActivate?.Invoke();
        }

        public void Deactivate(bool resetCurrentForce = false)
        {
            _isActive = false;
            _timeElapsed = 0f;
            if (resetCurrentForce) SetForce(Vector3.zero);

            OnForceDeactivate?.Invoke();
        }

        public void Update(float time)
        {
            
            if (_timeElapsed > _duration && _duration != -1)
            {
                Deactivate(_resetOnDeactivate);
            }

            if (_isActive)
            {
                Accelerate(time);
            }
            else
            {
                Decelerate(time);
            }

            _timeElapsed += time;

        }

        private void Accelerate(float time)
        {
            

            if(_forceAcceleration == -1)
            {
                _forceCurrent = _forceMax * _direction;
            }
            else
            {
                _forceCurrent += _direction * _forceAcceleration * time;
            }

            _forceCurrent = Vector3.ClampMagnitude(_forceCurrent, _forceMax);
        }

        private void Decelerate(float time)
        {
            if (_forceCurrent.magnitude > 0)
            {
                float addedMagnitude = _forceDeceleration * time;

                if (_forceCurrent.magnitude - addedMagnitude <= 0)
                {
                    SetForce(Vector3.zero);
                }
                else
                {
                    _forceCurrent += addedMagnitude * (-_forceCurrent);
                }
                _forceCurrent = Vector3.ClampMagnitude(_forceCurrent, _forceMax);
            }
        }

        public void SetForce(Vector3 force)
        {
            _forceCurrent = Vector3.ClampMagnitude(force, _forceMax);

            if (!_isActive && _forceCurrent.magnitude == 0)
            {
                OnForceEnd?.Invoke();
            }
        }
        public void SetDirection(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Activate(_resetOnActivate);
                _direction = direction.normalized;
            }
            else
            {
                Deactivate(_resetOnDeactivate);
            }
        }
    }
}
