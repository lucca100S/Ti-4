using UnityEngine;

namespace Physics
{
    public class PhysicsForcesDrawer : MonoBehaviour
    {
        private PhysicsController _physicsController;

        [SerializeField] private float _size;

        #region Properties

        public PhysicsController PhysicsController { get { return _physicsController; } }
        public float Size { get { return _size; } }

        #endregion

        private void Awake()
        {
            _physicsController = GetComponent<PhysicsController>();
        }


    }
}
