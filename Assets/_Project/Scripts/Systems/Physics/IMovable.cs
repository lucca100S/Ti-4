using UnityEngine;

namespace Physics
{
    public interface IMovable
    {
        public void SetForce(Vector3 force);
        public void AddForce(Vector3 force);
        public Vector3 CurrentVelocity { get; }
        public Vector3 CurrentDirection { get { return CurrentVelocity.normalized; } }

    }
}
