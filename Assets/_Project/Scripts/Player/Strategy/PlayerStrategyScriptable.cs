using UnityEngine;
using Player.Movement;
using System.Collections.Generic;

namespace Player.Strategy
{
    [System.Serializable]
    public abstract class PlayerStrategyScriptable : ScriptableObject
    {
        protected float _speed = 1;
        [SerializeField] protected float _jumpForce;
        [SerializeField] protected float _jumpCancelFactor;
        [SerializeField] protected float _gravity;
        [SerializeField] protected float _fallGravityFactor;

        [SerializeField] protected float _height = 1;
        [SerializeField] protected Vector3 _center = Vector3.zero;

        //TEMP DEBUG
        [SerializeField] protected Vector3 _scale = Vector3.one;

        [SerializeField] protected List<StrategyMaterialStats> _materialStats;

        #region Properties

        public abstract PlayerStrategyHandler.Strategy Strategy { get; protected set; }
        public abstract PlayerStrategyHandler.Strategy NextStrategy { get; protected set; }

        public float Speed { get { return _speed; } private set { _speed = value; } }
        public float JumpForce { get { return _jumpForce; } private set { _jumpForce= value; } }
        public float JumpCancelFactor { get { return _jumpCancelFactor; } }
        public float Gravity { get { return _gravity; } }
        public float FallGravityFactor { get { return _fallGravityFactor; } }

        public float Height { get { return _height; } private set { _height = value; } }
        public Vector3 Center { get { return _center; } private set { _center = value; } }
        public Vector3 Scale { get { return _scale; } private set { _scale = value; } }


        #endregion

        public abstract void Jump(PlayerMovement player);
        public abstract void Move(PlayerMovement player);
        public abstract void Transform(PlayerMovement player);
        public abstract void GetDirection(PlayerMovement player);
        public abstract void Rotate(PlayerMovement player);

        protected float GetWalkSpeed(PlayerMovement player)
        {
            return _materialStats[(int)player.CurrentMaterial].WalkSpeed;
        }

        protected float GetJumpSpeed(PlayerMovement player)
        {
            return _materialStats[(int)player.CurrentMaterial].JumpSpeed;
        }

        protected float GetClimbSpeed(PlayerMovement player)
        {
            return _materialStats[(int)player.CurrentMaterial].ClimbSpeed;
        }

    }
}
