using UnityEngine;

namespace Physics
{
    public interface IMovable
    {
        public void SetForce(Vector3 force);
        public void AddForce(Vector3 force);
        public void SetVelocity(Vector3 velocity);
        public void AddVelocity(Vector3 velocity);
        public Vector3 CurrentForce { get; }
        public Vector3 CurrentVelocity { get; }

    }
}
