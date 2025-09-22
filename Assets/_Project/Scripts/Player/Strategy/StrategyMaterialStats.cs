using System.Collections.Generic;
using UnityEngine;

namespace Player.Strategy
{
    [System.Serializable]
    public class StrategyMaterialStats
    {
        [SerializeField] private SurfaceMaterial _material;

        [SerializeField] private float _walkSpeed;
        [Tooltip("Speed when jumping from this material")] [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _climbSpeed;
        [SerializeField] private bool _canClimb = true;
        [SerializeField] private float _drag;


        #region Properties

        public SurfaceMaterial Material { get { return _material; } }


        public float WalkSpeed { get { return _walkSpeed; } private set { _walkSpeed = value; } }
        public float JumpSpeed { get { return _jumpSpeed; } private set { _jumpSpeed = value; } }
        public float ClimbSpeed { get { return _climbSpeed; } private set { _climbSpeed = value; } }
        public bool CanClimb { get { return _canClimb; } private set { _canClimb = value; } }

        public float Drag { get { return _drag; } private set { _drag = value; } }

        #endregion
    }
}
