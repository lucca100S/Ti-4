using UnityEngine;

namespace Player.StateMachine
{
    public abstract class PlayerState : ScriptableObject
    {
        [SerializeField] protected float _distanceTreshold;
        [SerializeField] protected SurfaceType _surfaceType;

        #region Properties

        public float DistanceTreshold { get { return _distanceTreshold; } }
        public SurfaceType SurfaceType { get { return _surfaceType; } }

        #endregion

        public abstract void Enter(PlayerController player);
        public abstract void Exit(PlayerController player);
        public abstract void StateUpdate(PlayerController player);
    }
}